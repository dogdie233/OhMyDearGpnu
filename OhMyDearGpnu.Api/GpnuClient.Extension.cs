using OhMyDearGpnu.Api.Modules;
using OhMyDearGpnu.Api.Requests;
using OhMyDearGpnu.Api.Responses;

namespace OhMyDearGpnu.Api
{
    public static class GpnuClientExtension
    {
        public static Task<Response> Login(this GpnuClient gpnuClient, string username, string password, Captcha captcha)
            => gpnuClient.SendRequest(new LoginRequest(username, password, captcha));

        public static Task<DataResponse<Captcha>> GetCaptcha(this GpnuClient gpnuClient)
            => gpnuClient.SendRequest(new GetCaptchaRequest());

        public static Task<DataResponse<PersonInfoResponse>> GetPersonInfo(this GpnuClient client)
            => client.SendRequest(new PersonInfoRequest());

        public static Task<DataResponse<Calendar>> GetCalendar(this GpnuClient client)
            => client.SendRequest(new GetCalendarRequest());

        public static Task<DataResponse<CurriculumInfo[]>> GetCurriculums(this GpnuClient client, int year, string termName)
            => client.SendRequest(new GetCurriculumInfosRequest(year, termName));
    }
}
