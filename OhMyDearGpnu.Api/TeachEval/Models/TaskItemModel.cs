using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public class TaskItemModel
{
    [JsonPropertyName("TaskId")] public string TaskId { get; set; } = string.Empty;

    [JsonPropertyName("AnsweredCount")] public int AnsweredCount { get; set; }

    [JsonPropertyName("TotalAnswerCount")] public int TotalAnswerCount { get; set; }

    [JsonPropertyName("QuestionnaireName")]
    public string QuestionnaireName { get; set; } = string.Empty;

    [JsonPropertyName("QuestionnaireId")] public string QuestionnaireId { get; set; } = string.Empty;

    [JsonPropertyName("EvaType")] public int EvaType { get; set; }

    [JsonPropertyName("EvaTypeName")] public string EvaTypeName { get; set; } = string.Empty;

    [JsonPropertyName("OwnerCode")] public string OwnerCode { get; set; } = string.Empty;

    [JsonPropertyName("OwnerName")] public string OwnerName { get; set; } = string.Empty;

    [JsonPropertyName("StartTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("EndTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("Status")] public int Status { get; set; }

    [JsonPropertyName("EvaCode")] public string EvaCode { get; set; } = string.Empty;

    [JsonPropertyName("IsAnonymous")] public int IsAnonymous { get; set; }

    [JsonPropertyName("IsShare")] public string IsShare { get; set; } = string.Empty;

    [JsonPropertyName("Semester")] public string Semester { get; set; } = string.Empty;

    [JsonPropertyName("TaskStatus")] public int TaskStatus { get; set; }

    [JsonPropertyName("IsEvalToCourse")] public int IsEvalToCourse { get; set; }

    [JsonPropertyName("IsMultipleAnswers")]
    public int IsMultipleAnswers { get; set; }

    [JsonPropertyName("IsEvalExportDisabled")]
    public bool IsEvalExportDisabled { get; set; }

    [JsonPropertyName("PublishItemId")] public int PublishItemId { get; set; }
}