using System.Text.Json.Serialization;
using OhMyDearGpnu.Api.TeachEval.Models;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public class GetStudentQuestionnaireHeaderAsyncRequest(StudentQuestionnairePageUrl page, string personCode) : TeachEvalRequest<StudentQuestionnaireHeaderModel>
{
    [JsonIgnore] public override Uri Url => new(Hosts.teachEval, "questionnaire/student/myTask/GetStudentQuestionnaireHeaderAsync");

    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;

    [JsonIgnore] public override string ApiName => "student/myTask/GetStudentQuestionnaireHeaderAsync";

    private readonly string _url = page.ToString();

    public string PersonCode { get; } = personCode;

    public string QuestionnaireId { get; } = page.QuestionnaireId;

    public string QId { get; } = page.QuestionnaireId;

    public string TeacherCode { get; } = page.TeacherCode;

    public string CourseCode { get; } = page.CourseCode;

    public string TypeCode { get; } = page.Type;

    public override string BuildRequestOriginPageAddress(SystemParamsModel model)
    {
        return _url;
    }
}