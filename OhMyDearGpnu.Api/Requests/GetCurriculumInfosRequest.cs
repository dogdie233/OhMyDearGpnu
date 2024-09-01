using OhMyDearGpnu.Api.Modules;
using OhMyDearGpnu.Api.Responses;

using System.Text.Json;

namespace OhMyDearGpnu.Api.Requests
{
    public class GetCurriculumInfosRequest : BaseWithDataResponseRequest<CurriculumInfo[]>
    {
        [FormItem("xnm")] public int year;
        [FormItem("xqm")] public string termName;
        [FormItem("kzlx")] private string kzlx = "ck";
        [FormItem("xsdm")] private string xsdm = "";

        public GetCurriculumInfosRequest(int year, string termName)
        {
            this.year = year;
            this.termName = termName;
        }

        public override string Path => "jwglxt/kbcx/xskbcx_cxXsgrkb.html";
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
                    Week = NumberRange.Parse(curriculumInfoElement.GetProperty("zcd").GetString()!.Replace("周", "")),
                    Name = curriculumInfoElement.GetProperty("kcmc").GetString() ?? string.Empty,
                    TimeId = NumberRange.Parse(curriculumInfoElement.GetProperty("jcs").GetString()!),
                    Campus = curriculumInfoElement.GetProperty("xqmc").GetString() ?? string.Empty,
                    TeacherName = curriculumInfoElement.GetProperty("xm").GetString() ?? string.Empty,
                    AssessmentTypeName = curriculumInfoElement.GetProperty("khfsmc").GetString() ?? string.Empty,
                    Comment = curriculumInfoElement.GetProperty("xkbz").GetString() ?? string.Empty,
                    CompositionOfCourseHours = curriculumInfoElement.GetProperty("kcxszc").GetString() ?? string.Empty,
                    WeeklyCourseHours = int.Parse(curriculumInfoElement.GetProperty("zhxs").GetString()!),
                    TotalCourseHours = int.Parse(curriculumInfoElement.GetProperty("zxs").GetString()!),
                    Credit = float.Parse(curriculumInfoElement.GetProperty("xf").GetString()!)
                };
                results.Add(curriculumInfo);
            }
            return DataResponse<CurriculumInfo[]>.Success(results.ToArray());
        }
    }
}
