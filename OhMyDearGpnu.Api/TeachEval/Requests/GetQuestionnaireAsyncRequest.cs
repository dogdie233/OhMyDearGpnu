using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.TeachEval.Models;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public class GetQuestionnaireAsyncRequest(StudentQuestionnairePageUrl page, int id, string typeCode) : TeachEvalRequest<QuestionnaireModel>
{
    [JsonIgnore] public override Uri Url => new(Hosts.teachEval, "questionnaire/public/GetQuestionnaireAsync");

    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;

    [JsonIgnore] public override string ApiName => "public/GetQuestionnaireAsync";

    private readonly string _url = page.ToString();

    public string Id { get; } = id.ToString();

    [JsonPropertyName("TypeCode")] public string TypeCode { get; } = typeCode;

    public override string BuildRequestOriginPageAddress(SystemParamsModel model)
    {
        return _url;
    }
}