using OhMyDearGpnu.Api.AcaAff;
using OhMyDearGpnu.Api.AcaAff.Requests;
using OhMyDearGpnu.Api.AcaAff.Responses;
using OhMyDearGpnu.Api.Cas;
using OhMyDearGpnu.Api.Cas.Requests;
using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api;

public static class GpnuClientExtension
{
    public static Task<Response> Login(this GpnuClient gpnuClient, string username, string password, Captcha captcha)
    {
        return gpnuClient.SendRequest(new LoginRequest(username, password, captcha));
    }

    public static async Task<CasHandler?> GetCasHandlerIfNecessary(this GpnuClient gpnuClient)
    {
        return !(await gpnuClient.SendRequest(new CheckCasRequest())).data ? null : new CasHandler(gpnuClient);
    }

    public static Task<DataResponse<Captcha>> GetCaptcha(this GpnuClient gpnuClient)
    {
        return gpnuClient.SendRequest(new GetCaptchaRequest());
    }

    public static Task<DataResponse<PersonInfoResponse>> GetPersonInfo(this GpnuClient client)
    {
        return client.SendRequest(new PersonInfoRequest());
    }

    public static Task<DataResponse<Calendar>> GetCalendar(this GpnuClient client)
    {
        return client.SendRequest(new GetCalendarRequest());
    }

    public static Task<DataResponse<CurriculumInfo[]>> GetCurriculums(this GpnuClient client, int year, string termName)
    {
        return client.SendRequest(new GetCurriculumInfosRequest(year, termName));
    }
}