using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;
using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public class GetMyTaskItemByAnswerStatusRequest : ApisDoRequestPaged<TaskItemModel>, ISystemParams
{
    [JsonIgnore] public override string ApiName => "Mycos.JP.MyTask.MyTask.GetMyTaskItemByAnswerStatus";

    public override string BuildRequestOriginPageAddress(SystemParamsModel model)
    {
        return $"{Hosts.teachEval}index.html?v=3.25.0#/my-task/main/{Status}";
    }

    public string Source { get; init; } = "pc";
    public string Status { get; init; } = "UnFinished";
    public int? EvaType { get; init; } = null;
    public string? EvaCode { get; init; } = null;

    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsIncludeHistorySemester { get; init; } = true;

    [JsonIgnore] public string? Semester { get; init; }
    [JsonIgnore] string? ISystemParams.Semester => Semester;
}

public class GetMyTaskItemDetailRequest(string taskId, string questionnaireType = "Final", string questionnaireTypeCode = "1", string? semester = null) : ApisDoRequestPaged<TaskItemDetailModel>, ISystemParams
{
    [JsonIgnore] public override string ApiName => "Mycos.JP.MyTask.MyTask.GetMyTaskItemDeail";

    public string TaskId { get; } = taskId;
    public string EvaType { get; } = questionnaireTypeCode;
    public string EvaCode { get; } = questionnaireType;

    [JsonIgnore] string? ISystemParams.Semester { get; } = semester;

    public override string BuildRequestOriginPageAddress(SystemParamsModel model)
    {
        return $"{Hosts.teachEval}index.html?v=3.25.0#/my-task/details/UnFinished/1/{EvaType}/{EvaCode}/undefined/{TaskId}?semester={model.Semester}";
    }
}

public class GetPublicKeyRequest(QuestionnairePageUrl? page = null) : ApisDoRequest<LoginKeysModel>
{
    [JsonIgnore] public override string ApiName => "Mycos.JP.Login.GetPublicKey";

    public override string BuildRequestOriginPageAddress(SystemParamsModel model)
    {
        return page?.ToString() ?? $"{Hosts.teachEval}index.html?v=3.25.0#/login";
    }
}

public class GetFinalQuestionnaireHeaderRequest(QuestionnairePageUrl page, string personCode, string courseCode)
    : ApisDoRequest<QuestionnaireHeaderModel>, ISystemParams
{
    private readonly string _originPage = page.ToString();

    [JsonIgnore] public override string ApiName => "Mycos.JP.Questionnaire.GetFinalQuestionnaireHeader";

    public string QuestionnaireId { get; } = page.QuestionnaireId;
    public string QId => QuestionnaireId;
    public string TaskId { get; } = page.TaskId;
    public string PersonCode { get; } = personCode;
    public string CourseCode { get; } = courseCode;
    public string QuestionnaireType { get; } = page.QuestionnaireType;

    [JsonIgnore] string? ISystemParams.Semester { get; } = page.Semester;

    public override string BuildRequestOriginPageAddress(SystemParamsModel model)
    {
        return _originPage;
    }
}

public class GetQuestionnaireRequest(QuestionnairePageUrl page) : ApisDoRequest<QuestionnaireModel>, ISystemParams
{
    private readonly string _originPage = page.ToString();

    [JsonIgnore] public override string ApiName => "Mycos.JP.Questionnaire.GetQuestionnaire";

    public string QuestionnaireType { get; } = page.QuestionnaireType;
    [JsonPropertyName("Id")] public string QuestionnaireId { get; } = page.QuestionnaireId;

    public override string BuildRequestOriginPageAddress(SystemParamsModel model)
    {
        return _originPage;
    }
}

public class SaveQuestionnaireAnswerRequest(QuestionnairePageUrl page) : ApisDoRequest<bool>, ISystemParams
{
    private readonly string _originPage = page.ToString();
    [JsonIgnore] public override string ApiName => "Mycos.JP.Questionnaire.SaveAnswer";

    public required int DetailId { get; init; }
    public string QuestionnaireId { get; } = page.QuestionnaireId;
    public string QuestionnaireType { get; } = page.QuestionnaireType;

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
        var key = await context.GpnuClient.SendRequest(new GetPublicKeyRequest(page));
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