using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.TeachEval.Models;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public class GetStudentTaskListAsyncRequest(QuestionnairePageUrl page, string questionnaireId, string personCode, int type, string search) : TeachEvalPagedRequest<StudentTaskModel>
{
    [JsonIgnore] public override Uri Url => new(Hosts.teachEval, "questionnaire/student/myTask/GetStudentTaskListAsync");

    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;

    [JsonIgnore] public override string ApiName => "student/myTask/GetStudentTaskListAsync";

    private readonly string _url = page.ToString();

    public string QuestionnaireId { get; } = questionnaireId;

    public string PersonCode { get; } = personCode;

    public int Type { get; } = type;

    [JsonPropertyName("search")] public string Search { get; } = search;

    public override string BuildRequestOriginPageAddress(SystemParamsModel model)
    {
        return _url;
    }
}