using System.Diagnostics.CodeAnalysis;

using OhMyDearGpnu.Api.Cas.Requests;
using OhMyDearGpnu.Api.Cas.Responses;

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

    public async Task<CasCaptcha> GetPasswordLoginCaptcha()
    {
        var res = await gpnuClient.SendRequest(new GetCasCaptchaRequest());
        if (!res.IsSucceeded)
            throw new Exception("Failed to get CAS captcha.");
        return res.data!;
    }

    public async Task<CasLoginResult> LoginByPassword(string username, string password, CasCaptcha casCaptcha, bool updateTgc = true, string? service = null)
    {
        service ??= defaultService;
        var ticketResponse = await gpnuClient.SendRequest(new LoginRequest(username, password, casCaptcha, service));
        if (!ticketResponse.IsSucceeded)
            return CasLoginResult.CreateFail(ticketResponse.message ?? "未知原因");

        var ticketError = TranslateTicketError(ticketResponse.data);
        if (ticketError is not null)
            return CasLoginResult.CreateFail(ticketError);

        if (ticketResponse.data is { Ticket: null } or { Tgt: null })
            return CasLoginResult.CreateFail("未知错误: Ticket 或 Tgt 为空");

        IsLoggedIn = true;
        Tgt = ticketResponse.data.Tgt;

        if (updateTgc)
        {
            await Task.Delay(1000); // 给学校的土豆休息一下
            await UpdateWebAuthCookie(gpnuClient.client);
        }

        return CasLoginResult.CreateSuccess(ticketResponse.data.Ticket, ticketResponse.data.Tgt);
    }

    public async Task LoginByTgt(string tgt, bool updateTgc = true)
    {
        if (!await CheckTgtValid(tgt))
            throw new CasTgtInvalidException();
        IsLoggedIn = true;
        Tgt = tgt;

        if (updateTgc)
            await UpdateWebAuthCookie(gpnuClient.client);
    }

    public async Task UpdateWebAuthCookie(HttpClient httpClient)
    {
        EnsureLoggedIn();
        var st = await GenerateServiceTicket(webAuthLoginService);
        var reqMsg = new HttpRequestMessage(HttpMethod.Get, $"{webAuthLoginService}&ticket={st}");
        var req = await httpClient.SendAsync(reqMsg);
        req.EnsureSuccessStatusCode();
    }

    public async Task<string> GenerateServiceTicket(string service)
    {
        EnsureLoggedIn();
        var res = await gpnuClient.SendRequest(new GetServiceTicketRequest(Tgt, service));
        if (!res.IsSucceeded)
            throw new CasTgtInvalidException(res.message ?? "Failed to generate service ticket.");
        return res.data;
    }

    [MemberNotNull(nameof(Tgt))]
    public void EnsureLoggedIn()
    {
        if (!IsLoggedIn)
            throw new CasNotLoggedInException();
    }

    private async Task<bool> CheckTgtValid(string tgt)
    {
        var res = await gpnuClient.SendRequest(new GetServiceTicketRequest(tgt, defaultService));
        return res.IsSucceeded;
    }

    private static string? TranslateTicketError(LoginResponse ticketResponse)
    {
        if (ticketResponse.Data is null)
            return null;

        var data = ticketResponse.Data;
        return data.Code switch
        {
            "CODEFALSE" => "验证码错误",
            "PASSERROR" => $"密码错误，已尝试 {data.Data[(data.Data.IndexOf(',') + 1) ..]} 次，总共可尝试 {..data.Data.IndexOf(',')} 次",
            "NOUSER" => "用户不存在",
            _ => $"未知的错误: Code {data.Code}, Data: {data.Data}"
        };
    }
}