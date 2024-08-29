using OhMyDearGpnu.Api.Requests.Cas;

namespace OhMyDearGpnu.Api.Modules.Cas;

public class CasHandler
{
    private readonly GpnuClient gpnuClient;
    
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

    public void LoginByPassword(string username, string password, CasCaptcha casCaptcha)
    {
        
    }
}
