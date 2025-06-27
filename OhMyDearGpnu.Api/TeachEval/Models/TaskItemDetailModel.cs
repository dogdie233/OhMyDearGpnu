using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public class TaskItemDetailModel
{
    [JsonPropertyName("Semester")] public string Semester { get; set; } = string.Empty;

    [JsonPropertyName("ClassCode")] public string ClassCode { get; set; } = string.Empty;

    [JsonPropertyName("CourseCode")] public string CourseCode { get; set; } = string.Empty;

    [JsonPropertyName("CourseName")] public string CourseName { get; set; } = string.Empty;

    [JsonPropertyName("ClassNo")] public string ClassNo { get; set; } = string.Empty;

    [JsonPropertyName("TeacherNames")] public string TeacherNames { get; set; } = string.Empty;

    [JsonPropertyName("TaskStatus")] public int TaskStatus { get; set; }

    [JsonPropertyName("HasScoreLevel")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool HasScoreLevel { get; set; }

    [JsonPropertyName("EvaType")] public int EvaType { get; set; }

    [JsonPropertyName("PersonCode")] public string PersonCode { get; set; } = string.Empty;

    [JsonPropertyName("QuestionnaireId")] public string QuestionnaireId { get; set; } = string.Empty;

    [JsonPropertyName("QuestionnaireName")]
    public string QuestionnaireName { get; set; } = string.Empty;

    [JsonPropertyName("Status")] public int Status { get; set; }

    [JsonPropertyName("StartTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("EndTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("TimeType")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool TimeType { get; set; }
}