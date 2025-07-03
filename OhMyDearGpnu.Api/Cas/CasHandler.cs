using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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

    public async Task<(string ticket, string tgt)> LoginByWechat(bool updateTgc = true, string? service = null)
    {
        var qRcodeResponse = await CreateWechatCaptcha();
        var checkScanRequest = new CheckScanRequest();
        checkScanRequest.state = qRcodeResponse.Uuid;

        var timeout = TimeSpan.FromMinutes(5);
        var startTime = DateTime.UtcNow;

        LoginResponse ticketResponse;
        CheckScanResponse checkScanResponse;
        while (true)
        {
            if (DateTime.UtcNow - startTime > timeout)
            {
                throw new TimeoutException("微信登录超时");
            }

            checkScanResponse = await gpnuClient.SendRequest(checkScanRequest);
            if (checkScanResponse.Data is null)
            {
                await Task.Delay(1000);
                continue;
            }
            break;
        }

        service = defaultService;
        ticketResponse = await gpnuClient.SendRequest(new LoginRequest(checkScanResponse.Data.UserName, checkScanResponse.Data.Password, service));
        EnsureLoginSuccess(ticketResponse);

        IsLoggedIn = true;
        Tgt = ticketResponse.Tgt!;

        if (updateTgc)
        {
            await Task.Delay(1000);
            await UpdateWebAuthCookie(gpnuClient.client);
        }


        return (ticketResponse.Ticket!, ticketResponse.Tgt!);
    }

    public async Task<CreateQRcodeResponse> CreateWechatCaptcha()
    {
        var qRcodeResponse = await gpnuClient.SendRequest(new CreateQRcodeRequest());
        if (qRcodeResponse?.Content == null || qRcodeResponse.Content.Length == 0)
        {
            throw new InvalidOperationException("二维码内容为空或无效");
        }

        var imageData = qRcodeResponse.Content;
        const string filePath = "casWechatCaptcha.png";
        File.Delete(filePath);
        using (var fs = File.Create(filePath))
        {
            await fs.WriteAsync(imageData);
            Console.WriteLine($"二维码已保存至: {Path.GetFullPath(filePath)}");
        }

        return qRcodeResponse;
    }

    public Task<CasCaptcha> GetPasswordLoginCaptcha()
    {
        return gpnuClient.SendRequest(new GetCasCaptchaRequest());
    }

    public async Task<(string ticket, string tgt)> LoginByPassword(string username, string password, CasCaptcha casCaptcha, bool updateTgc = true, string? service = null)
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

    public Task<string> GenerateServiceTicket(string service)
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

    private async Task<bool> CheckTgtValid(string tgt)
    {
        try
        {
            await gpnuClient.SendRequest(new GetServiceTicketRequest(tgt, defaultService));
        }
        catch (UnexpectedResponseException)
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
                "PASSERROR" => (CasLoginFailException.LoginFailReasonType.PassError, $"密码错误，已尝试 {data.Data[(data.Data.IndexOf(',') + 1)..]} 次，总共可尝试 {data.Data[..data.Data.IndexOf(',')]} 次"),
                "NOUSER" => (CasLoginFailException.LoginFailReasonType.NoUser, "用户不存在"),
                _ => (CasLoginFailException.LoginFailReasonType.Unknown, $"未知的错误: Code {data.Code}, Data: {data.Data}")
            };
            throw new CasLoginFailException(info.Item1, info.Item2);
        }

        if (ticketResponse is { Ticket: null } or { Tgt: null })
            throw new UnexpectedResponseException("ticket or tgt is null");
    }
}