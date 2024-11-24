namespace OhMyDearGpnu.Api.Cas;

public static class GpnuClientCasExtension
{
    public static Task<CasCaptcha> CasGetLoginCaptcha(this GpnuClient client)
    {
        return client.cas.GetPasswordLoginCaptcha();
    }

    public static Task<CasLoginResult> CasLoginByPassword(this GpnuClient client, string username, string password, CasCaptcha casCaptcha)
    {
        return client.cas.LoginByPassword(username, password, casCaptcha);
    }
}