using OhMyDearGpnu.Api.AcaAff.Requests;
using OhMyDearGpnu.Api.AcaAff.Responses;
using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.AcaAff;

public static class GpnuClientAcaAffExtension
{
    public static Task<Response> AcaAffLogin(this GpnuClient gpnuClient, string username, string password, Captcha captcha)
    {
        return gpnuClient.SendRequest(new LoginRequest(username, password, captcha));
    }

    public static Task<DataResponse<Captcha>> AcaAffGetCaptcha(this GpnuClient gpnuClient)
    {
        return gpnuClient.SendRequest(new GetCaptchaRequest());
    }

    public static Task<DataResponse<PersonInfoResponse>> AcaAffGetPersonInfo(this GpnuClient client)
    {
        return client.SendRequest(new PersonInfoRequest());
    }

    public static Task<DataResponse<Calendar>> AcaAffGetCalendar(this GpnuClient client)
    {
        return client.SendRequest(new GetCalendarRequest());
    }

    public static Task<DataResponse<CurriculumInfo[]>> AcaAffGetCurriculums(this GpnuClient client, int year, string termName)
    {
        return client.SendRequest(new GetCurriculumInfosRequest(year, termName));
    }
}