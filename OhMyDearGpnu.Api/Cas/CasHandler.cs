using System.Diagnostics.CodeAnalysis;

using OhMyDearGpnu.Api.Cas.Requests;
using OhMyDearGpnu.Api.Cas.Responses;
using OhMyDearGpnu.Api.Common.Drawing;

namespace OhMyDearGpnu.Api.Cas;

public class CasHandler
{
    private readonly GpnuClient gpnuClient;
    public readonly string defaultService = "https://portal.gpnu.edu.cn/shiro-cas";
    public readonly string webAuthLoginService = Hosts.webAuth + "wengine-auth/login?cas_login=true";

    [MemberNotNullWhen(true, nameof(Tgt))] public bool IsLoggedIn { get; private set; }
    public string? Tgt { get; private set; } = null;

    internal CasHandler(GpnuClient gpnuClient)
    {
        this.gpnuClient = gpnuClient;
    }

    // TODO: unimplemented
    // public void LoginByWorkWechat()
    // {
    //    
    // }

    public ValueTask<CasCaptcha> GetPasswordLoginCaptcha()
    {
        return gpnuClient.SendRequest(new GetCasCaptchaRequest());
    }

    public async ValueTask<(string ticket, string tgt)> LoginByPassword(string username, string password, CasCaptcha casCaptcha, bool updateTgc = true, string? service = null)
    {
        if (casCaptcha.value is null)
        {
            var captchaResolver = gpnuClient.serviceContainer.Locate<ICasCaptchaResolver>();
            casCaptcha.value = await captchaResolver.ResolveCaptchaAsync(Image.FromSimplePng(casCaptcha.image));
        }

        service ??= defaultService;
        var ticketResponse = await gpnuClient.SendRequest(new LoginRequest(username, password, casCaptcha, service));

        EnsureLoginSuccess(ticketResponse);

        IsLoggedIn = true;
        Tgt = ticketResponse.Tgt!;

        if (updateTgc)
        {
            await Task.Delay(1000); // 给学校的土豆休息一下
            await UpdateWebAuthCookie(gpnuClient.client);
        }

        return (ticketResponse.Ticket!, ticketResponse.Tgt!);
    }

    public async ValueTask LoginByTgt(string tgt, bool updateTgc = true)
    {
        if (!await CheckTgtValid(tgt))
            throw new CasTgtInvalidException();

        IsLoggedIn = true;
        Tgt = tgt;

        if (updateTgc)
            await UpdateWebAuthCookie(gpnuClient.client);
    }

    public async ValueTask UpdateWebAuthCookie(HttpClient httpClient)
    {
        EnsureLoggedIn();
        var st = await GenerateServiceTicket(webAuthLoginService);
        var reqMsg = new HttpRequestMessage(HttpMethod.Get, $"{webAuthLoginService}&ticket={st}");
        var req = await httpClient.SendAsync(reqMsg);
        req.EnsureSuccessStatusCode();
    }

    public ValueTask<string> GenerateServiceTicket(string service)
    {
        EnsureLoggedIn();
        return gpnuClient.SendRequest(new GetServiceTicketRequest(Tgt, service));
    }

    [MemberNotNull(nameof(Tgt))]
    public void EnsureLoggedIn()
    {
        if (!IsLoggedIn)
            throw new CasNotLoggedInException();
    }

    private async ValueTask<bool> CheckTgtValid(string tgt)
    {
        try
        {
            await gpnuClient.SendRequest(new GetServiceTicketRequest(tgt, defaultService));
        }
        catch (UnexpectedResponseException e)
        {
            return false;
        }

        return true;
    }

    private static void EnsureLoginSuccess(LoginResponse ticketResponse)
    {
        if (ticketResponse.Data is not null)
        {
            var data = ticketResponse.Data;
            var info = data.Code switch
            {
                "CODEFALSE" => (CasLoginFailException.LoginFailReasonType.CodeFalse, "验证码错误"),
                "PASSERROR" => (CasLoginFailException.LoginFailReasonType.PassError, $"密码错误，已尝试 {data.Data[(data.Data.IndexOf(',') + 1) ..]} 次，总共可尝试 {..data.Data.IndexOf(',')} 次"),
                "NOUSER" => (CasLoginFailException.LoginFailReasonType.NoUser, "用户不存在"),
                _ => (CasLoginFailException.LoginFailReasonType.Unknown, $"未知的错误: Code {data.Code}, Data: {data.Data}")
            };
            throw new CasLoginFailException(info.Item1, info.Item2);
        }

        if (ticketResponse is { Ticket: null } or { Tgt: null })
            throw new UnexpectedResponseException("ticket or tgt is null");
    }
}