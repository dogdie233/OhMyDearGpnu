using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public class StudentQuestionnaireHeaderModel
{
    [JsonPropertyName("Props")] public QuestionnaireProps Props { get; set; } = new();

    // [JsonPropertyName("Items")] public List<object> Items { get; set; } = new();

    // [JsonPropertyName("PositionOrder")] public List<object> PositionOrder { get; set; } = new();

    [JsonPropertyName("OwnerName")] public string OwnerName { get; set; } = string.Empty;

    [JsonPropertyName("DetailId")] public int DetailId { get; set; }

    [JsonPropertyName("ResultId")] public int ResultId { get; set; }

    [JsonPropertyName("AnswerCount")] public int AnswerCount { get; set; }

    [JsonPropertyName("IsMultipleAnswers")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsMultipleAnswers { get; set; }

    [JsonPropertyName("IsScoreAvailable")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsScoreAvailable { get; set; }

    [JsonPropertyName("QuestionnaireStatus")] public int QuestionnaireStatus { get; set; }

    [JsonPropertyName("AnswerStatus")] public int AnswerStatus { get; set; }

    [JsonPropertyName("IsEvalToCourse")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsEvalToCourse { get; set; }

    [JsonPropertyName("IsTeachingData")] public bool IsTeachingData { get; set; }

    [JsonPropertyName("IsDownloadData")] public bool IsDownloadData { get; set; }

    [JsonPropertyName("TaskClassificationEvalMode")] public int TaskClassificationEvalMode { get; set; }

    [JsonPropertyName("Id")] public int Id { get; set; }

    [JsonPropertyName("QuestionnaireId")] public int QuestionnaireId { get; set; }

    [JsonPropertyName("TaskId")] public int TaskId { get; set; }

    [JsonPropertyName("Name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("OwnerCode")] public string OwnerCode { get; set; } = string.Empty;

    [JsonPropertyName("CreateTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime CreateTime { get; set; }

    [JsonPropertyName("UpdateTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    [JsonPropertyName("IsIncludeFeedback")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsIncludeFeedback { get; set; }

    [JsonPropertyName("Status")] public int Status { get; set; }

    [JsonPropertyName("ClientType")] public int ClientType { get; set; }

    [JsonPropertyName("TypeCode")] public string TypeCode { get; set; } = string.Empty;

    [JsonPropertyName("TypeName")] public string TypeName { get; set; } = string.Empty;

    [JsonPropertyName("IsScore")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsScore { get; set; }

    [JsonPropertyName("Version")] public int Version { get; set; }

    [JsonPropertyName("StartTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("EndTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("IsAnonymous")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsAnonymous { get; set; }

    [JsonPropertyName("ReferenceId")] public int ReferenceId { get; set; }

    [JsonPropertyName("ShareSetting")] public string ShareSetting { get; set; } = string.Empty;

    [JsonPropertyName("IsSendWxMsgToTh")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsSendWxMsgToTh { get; set; }

    [JsonPropertyName("ReferenceType")] public int ReferenceType { get; set; }

    [JsonPropertyName("SendTimeFilter")] public bool SendTimeFilter { get; set; }
}