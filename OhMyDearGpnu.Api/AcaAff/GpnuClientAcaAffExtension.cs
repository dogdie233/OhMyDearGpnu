using OhMyDearGpnu.Api.AcaAff.Requests;
using OhMyDearGpnu.Api.AcaAff.Responses;
using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.AcaAff;

public static class GpnuClientAcaAffExtension
{
    public static ValueTask AcaAffLogin(this GpnuClient gpnuClient, string username, string password, Captcha captcha)
    {
        return gpnuClient.SendRequest(new LoginRequest(username, password, captcha));
    }

    public static ValueTask<Captcha> AcaAffGetCaptcha(this GpnuClient gpnuClient)
    {
        return gpnuClient.SendRequest(new GetCaptchaRequest());
    }

    public static ValueTask<PersonInfoResponse> AcaAffGetPersonInfo(this GpnuClient client)
    {
        return client.SendRequest(new PersonInfoRequest());
    }

    public static ValueTask<Calendar> AcaAffGetCalendar(this GpnuClient client)
    {
        return client.SendRequest(new GetCalendarRequest());
    }

    public static ValueTask<CurriculumInfo[]> AcaAffGetCurriculums(this GpnuClient client, int year, string termName)
    {
        return client.SendRequest(new GetCurriculumInfosRequest(year, termName));
    }
}