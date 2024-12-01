using OhMyDearGpnu.Api.AcaAff.Requests;
using OhMyDearGpnu.Api.AcaAff.Responses;
using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.AcaAff;

public static class AcaAffContextExtension
{
    public static Task<PersonInfoResponse> GetPersonInfo(this AcaAffContext context)
    {
        return context.Client.SendRequest(new PersonInfoRequest());
    }

    public static Task<Calendar> GetCalendar(this AcaAffContext context)
    {
        return context.Client.SendRequest(new GetCalendarRequest());
    }

    public static Task<CurriculumInfo[]> GetCurriculums(this AcaAffContext context, int year, string termName)
    {
        return context.Client.SendRequest(new GetCurriculumInfosRequest(year, termName));
    }
}