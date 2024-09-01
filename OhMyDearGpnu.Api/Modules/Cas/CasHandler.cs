using System.Diagnostics.CodeAnalysis;

using OhMyDearGpnu.Api.Requests.Cas;
using OhMyDearGpnu.Api.Responses.Cas;

namespace OhMyDearGpnu.Api.Modules.Cas;

public class CasHandler
{
    private readonly GpnuClient gpnuClient;
    
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

    public async Task<string?> LoginByPassword(string username, string password, CasCaptcha casCaptcha, bool updateTgc = true)
    {
        var ticketResponse = await gpnuClient.SendRequest(new PostTicketRequest(username, password, casCaptcha));
        if (!ticketResponse.IsSucceeded)
            return ticketResponse.message ?? "未知原因";
        
        var ticketError = TranslateTicketError(ticketResponse.data!);
        if (ticketError is not null)
            return ticketError;
        
        if (ticketResponse.data is { Ticket: null } or { Tgt: null })
            return "未知错误: Ticket 或 Tgt 为空";

        IsLoggedIn = true;
        gpnuClient.serviceContainer.AddExisted(this);
        Tgt = ticketResponse.data.Tgt;

        if (updateTgc)
        {
            await Task.Delay(1000);  // 给学校的土豆休息一下
            await UpdateHttpClientTgc(gpnuClient.client);
        }
            
        return null;
    }
    
    public async Task LoginByTgt(string tgt, bool updateTgc = true)
    {
        IsLoggedIn = true;
        gpnuClient.serviceContainer.AddExisted(this);
        Tgt = tgt;

        if (updateTgc)
            await UpdateHttpClientTgc(gpnuClient.client);
    }

    public async Task UpdateHttpClientTgc(HttpClient httpClient)
    {
        var st = await GenerateServiceTicket();
        var reqMsg = new HttpRequestMessage(HttpMethod.Get, $"https://webauth.gpnu.edu.cn/wengine-auth/login?cas_login=true&ticket={st}");
        var req = await httpClient.SendAsync(reqMsg);
        req.EnsureSuccessStatusCode();
    }

    public async Task<string> GenerateServiceTicket()
    {
        EnsureLoggedIn();
        var res = await gpnuClient.SendRequest(new GetServiceTicketRequest(Tgt));
        if (!res.IsSucceeded)
            throw new Exception(res.message);

        return res.data;
    }

    [MemberNotNull(nameof(Tgt))]
    private void EnsureLoggedIn()
    {
        if (!IsLoggedIn)
            throw new CasNotLoggedInException();
    }

    private static string? TranslateTicketError(PostTicketResponse ticketResponse)
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
