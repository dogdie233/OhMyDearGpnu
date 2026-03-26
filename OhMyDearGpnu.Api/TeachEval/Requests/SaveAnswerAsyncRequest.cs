using System.Text.Json;
using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;
using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public class SaveAnswerAsyncRequest(StudentQuestionnairePageUrl page) : TeachEvalRequest<bool>, ISystemParams
{
    [JsonIgnore] public override Uri Url => new(Hosts.teachEval, "questionnaire/public/SaveAnswerAsync");

    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;

    [JsonIgnore] public override string ApiName => "public/SaveAnswerAsync";

    private readonly string _originPage = page.ToString();

    public required int DetailId { get; init; }

    public string QId { get; } = page.QuestionnaireId;

    public string TeacherCode { get; } = page.TeacherCode;

    public string CourseCode { get; } = page.CourseCode;

    public required int ResultId { get; init; }

    public string TypeCode { get; } = page.Type;

    public string QuestionnaireId { get; } = page.QuestionnaireId;

    public string QuestionnaireType { get; } = page.Type;

    public required int Version { get; init; }

    public required string PersonCode { get; init; }

    public required int TotalAnsweredSecond { get; init; }

    public int ClientType { get; set; } = 0;

    public required List<SubjectAnswerModel> Subjects { get; init; }

    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsChange { get; init; }

    public string? AuthKey { get; internal set; } = null!;

    [JsonIgnore] string? ISystemParams.Semester { get; } = page.Semester;

    public override string BuildRequestOriginPageAddress(SystemParamsModel model)
    {
        return _originPage;
    }

    public override async ValueTask FillAutoFieldAsync(SimpleServiceContainer serviceContainer)
    {
        if (AuthKey is not null)
            return;

        var context = serviceContainer.Locate<TeachEvalContext>();
        var key = await context.GpnuClient.SendRequest(new GetPublicKeyRequest(page.ToString()));
        var publicExponent = Convert.FromHexString(key.PublicKey);
        var publicModulus = Convert.FromHexString(key.PublicValue);
        AuthKey = EncryptHelper.TeachEvalSaveEncrypt($"{DetailId}&{PersonCode}", publicExponent, publicModulus);
    }

    public override async ValueTask<bool> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        var responseDocument = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
        var root = responseDocument.RootElement;
        return root.GetProperty("Value")!.GetBoolean();
    }
}