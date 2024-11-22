using OhMyDearGpnu.Api.Modules;
using OhMyDearGpnu.Api.Responses;

using System.Text.Json;

namespace OhMyDearGpnu.Api.Requests;

[Request(PayloadTypeEnum.FormUrlEncoded)]
public partial class GetCurriculumInfosRequest : BaseWithDataResponseRequest<CurriculumInfo[]>
{
    public override Uri Url => new(Hosts.acaAff, "jwglxt/kbcx/xskbcx_cxXsgrkb.html");

    [FormItem("xnm")] private int Year { get; init; }
    [FormItem("xqm")] public string TermName { get; init; }
    [FormItem("kzlx")] private string Kzlx => "ck";
    [FormItem("xsdm")] private string Xsdm => "";

    public GetCurriculumInfosRequest(int year, string termName)
    {
        Year = year;
        TermName = termName;
    }

    public override HttpMethod HttpMethod => HttpMethod.Post;

    public override async Task<DataResponse<CurriculumInfo[]>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        using var document = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
        var root = document.RootElement;
        var results = new List<CurriculumInfo>();
        var curriculumsArray = root.GetProperty("kbList");
        foreach (var curriculumInfoElement in curriculumsArray.EnumerateArray())
        {
            var curriculumInfo = new CurriculumInfo()
            {
                Classroom = curriculumInfoElement.GetProperty("cdmc").GetString() ?? string.Empty,
                Day = int.Parse(curriculumInfoElement.GetProperty("xqj").GetString()!),
                WeekString = curriculumInfoElement.GetProperty("zcd").GetString() ?? string.Empty,
                Name = curriculumInfoElement.GetProperty("kcmc").GetString() ?? string.Empty,
                TimeIdString = curriculumInfoElement.GetProperty("jcdm").GetString() ?? string.Empty,
                Campus = curriculumInfoElement.GetProperty("xqmc").GetString() ?? string.Empty,
                TeacherName = curriculumInfoElement.GetProperty("xm").GetString() ?? string.Empty,
                AssessmentTypeName = curriculumInfoElement.GetProperty("khfsmc").GetString() ?? string.Empty,
                Comment = curriculumInfoElement.GetProperty("xkbz").GetString() ?? string.Empty,
                CompositionOfCourseHours = curriculumInfoElement.GetProperty("kcxszc").GetString() ?? string.Empty,
                WeeklyCourseHours = int.Parse(curriculumInfoElement.GetProperty("zhxs").GetString()!),
                TotalCourseHours = int.Parse(curriculumInfoElement.GetProperty("zxs").GetString()!),
                Credit = float.Parse(curriculumInfoElement.GetProperty("xf").GetString()!)
            };
            curriculumInfo.TimeId = DiscreteNumberRange.Parse(curriculumInfo.TimeIdString);
            curriculumInfo.Week = DiscreteNumberRange.Parse(curriculumInfo.WeekString.Replace("周", ""));
            results.Add(curriculumInfo);
        }

        return DataResponse<CurriculumInfo[]>.Success(results.ToArray());
    }
}