using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public class QuestionnaireHeaderModel
{
    [JsonPropertyName("CourseList")] public List<CourseListItem> CourseList { get; set; } = new();

    // [JsonPropertyName("AnswerTimeInfos")] public List<object> AnswerTimeInfos { get; set; } = new();

    [JsonPropertyName("Props")] public QuestionnaireProps Props { get; set; } = new();

    [JsonPropertyName("Version")] public int Version { get; set; }

    [JsonPropertyName("FinalAssistantId")] public int FinalAssistantId { get; set; }

    [JsonPropertyName("Id")] public int Id { get; set; }

    [JsonPropertyName("Name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("OwnerCode")] public string OwnerCode { get; set; } = string.Empty;

    [JsonPropertyName("OwnerName")] public string OwnerName { get; set; } = string.Empty;

    [JsonPropertyName("QuestionnaireStatus")]
    public int QuestionnaireStatus { get; set; }

    [JsonPropertyName("ItemRangeStatus")] public int ItemRangeStatus { get; set; }

    [JsonPropertyName("CreateTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime CreateTime { get; set; }

    [JsonPropertyName("StartTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("EndTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("IsScore")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsScore { get; set; }

    [JsonPropertyName("TimeType")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool TimeType { get; set; }

    [JsonPropertyName("IsTeacher")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsTeacher { get; set; }

    [JsonPropertyName("IsEvaluateCourse")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsEvaluateCourse { get; set; }

    [JsonPropertyName("IsEvalToCourse")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsEvalToCourse { get; set; }

    [JsonPropertyName("HasEnterConfirmText")]
    public bool HasEnterConfirmText { get; set; }

    [JsonPropertyName("HasSubmitConfirmText")]
    public bool HasSubmitConfirmText { get; set; }

    [JsonPropertyName("IsScoreAvailable")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsScoreAvailable { get; set; }

    [JsonPropertyName("IsScoreLevel")] public bool IsScoreLevel { get; set; }

    [JsonPropertyName("IsScoreLevelRate")] public bool IsScoreLevelRate { get; set; }

    [JsonPropertyName("ScoreLevelInterval")]
    public int ScoreLevelInterval { get; set; }

    // [JsonPropertyName("ScoreLevelConfig")] public List<object> ScoreLevelConfig { get; set; } = new();

    [JsonPropertyName("OnVideo")] public bool OnVideo { get; set; }

    [JsonPropertyName("MaxScore")] public double MaxScore { get; set; }
}

public class CourseListItem
{
    [JsonPropertyName("CourseList")] public CourseInfo CourseList { get; set; } = new();

    [JsonPropertyName("TeacherList")] public List<TeacherInfo> TeacherList { get; set; } = new();

    [JsonPropertyName("TeachingAssistantList")]
    public List<object> TeachingAssistantList { get; set; } = new();

    [JsonPropertyName("CourseCode")] public string CourseCode { get; set; } = string.Empty;

    [JsonPropertyName("CourseName")] public string CourseName { get; set; } = string.Empty;
}

public class CourseInfo
{
    [JsonPropertyName("Name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Id")] public int Id { get; set; }

    [JsonPropertyName("TypeCode")] public string TypeCode { get; set; } = string.Empty;
}

public class TeacherInfo
{
    [JsonPropertyName("ClassCode")] public string ClassCode { get; set; } = string.Empty;

    [JsonPropertyName("TeacherCode")] public string TeacherCode { get; set; } = string.Empty;

    [JsonPropertyName("TeacherName")] public string TeacherName { get; set; } = string.Empty;

    [JsonPropertyName("AvatarUrl")] public string AvatarUrl { get; set; } = string.Empty;

    [JsonPropertyName("DetailId")] public int DetailId { get; set; }

    [JsonPropertyName("AnswerStatus")] public int AnswerStatus { get; set; }

    [JsonPropertyName("StartTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("EndTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("Id")] public int Id { get; set; }

    [JsonPropertyName("TypeCode")] public string TypeCode { get; set; } = string.Empty;
}

public class QuestionnaireProps
{
    [JsonPropertyName("Questionnaireid")] public int Questionnaireid { get; set; }

    [JsonPropertyName("CustomAttr")] public string CustomAttr { get; set; } = string.Empty;

    [JsonPropertyName("TopDesc")] public string TopDesc { get; set; } = string.Empty;

    [JsonPropertyName("FinishDesc")] public string FinishDesc { get; set; } = string.Empty;
}