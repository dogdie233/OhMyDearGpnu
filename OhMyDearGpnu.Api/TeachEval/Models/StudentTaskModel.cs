using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public class StudentTaskModel
{
    [JsonPropertyName("QuestionnaireId")] public int QuestionnaireId { get; set; }

    [JsonPropertyName("ResultId")] public int ResultId { get; set; }

    [JsonPropertyName("RevokeId")] public int RevokeId { get; set; }

    [JsonPropertyName("TeacherCode")] public string TeacherCode { get; set; } = string.Empty;

    [JsonPropertyName("TeacherName")] public string TeacherName { get; set; } = string.Empty;

    [JsonPropertyName("CourseCode")] public string CourseCode { get; set; } = string.Empty;

    [JsonPropertyName("CourseName")] public string CourseName { get; set; } = string.Empty;

    [JsonPropertyName("TypeCode")] public string TypeCode { get; set; } = string.Empty;

    [JsonPropertyName("TypeName")] public string TypeName { get; set; } = string.Empty;

    [JsonPropertyName("IsEvalToCourse")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsEvalToCourse { get; set; }

    [JsonPropertyName("IsMultipleAnswers")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsMultipleAnswers { get; set; }

    [JsonPropertyName("StartTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("EndTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("EvaluationCount")] public int EvaluationCount { get; set; }
}