using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public class QuestionnaireModel
{
    [JsonPropertyName("Props")] public QuestionnaireProps Props { get; set; } = new();

    [JsonPropertyName("Items")] public List<QuestionnaireItem> Items { get; set; } = new();

    [JsonPropertyName("PositionOrder")] public List<int> PositionOrder { get; set; } = new();

    [JsonPropertyName("Id")] public int Id { get; set; }

    [JsonPropertyName("Name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("OwnerCode")] public string OwnerCode { get; set; } = string.Empty;

    [JsonPropertyName("CreateTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime CreateTime { get; set; }

    [JsonPropertyName("UpdateTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    [JsonPropertyName("StartTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("EndTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("IsIncludeFeedback")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsIncludeFeedback { get; set; }

    [JsonPropertyName("Type")] public string Type { get; set; } = string.Empty;

    [JsonPropertyName("TypeName")] public string TypeName { get; set; } = string.Empty;

    [JsonPropertyName("ReferenceCount")] public int ReferenceCount { get; set; }

    [JsonPropertyName("IsScore")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsScore { get; set; }

    [JsonPropertyName("ClientType")] public int ClientType { get; set; }

    [JsonPropertyName("TimeType")] public int TimeType { get; set; }

    [JsonPropertyName("Version")] public int Version { get; set; }

    [JsonPropertyName("QuestionnaireTable")]
    public string QuestionnaireTable { get; set; } = string.Empty;

    [JsonPropertyName("FinalAssistantId")] public int FinalAssistantId { get; set; }

    [JsonPropertyName("IsSchool")] public bool IsSchool { get; set; }

    [JsonPropertyName("IsTeacher")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsTeacher { get; set; }

    [JsonPropertyName("IsFullMark")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsFullMark { get; set; }

    [JsonPropertyName("IsChoosableSameIndex")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsChoosableSameIndex { get; set; }

    [JsonPropertyName("IsScoreAvailable")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsScoreAvailable { get; set; }

    [JsonPropertyName("EvaluationIdent")] public int EvaluationIdent { get; set; }
}

public class QuestionnaireItem
{
    // [JsonPropertyName("PositionOrder")] public List<object> PositionOrder { get; set; } = new();

    [JsonPropertyName("Props")] public ItemProps Props { get; set; } = new();

    [JsonPropertyName("Id")] public int Id { get; set; }

    [JsonPropertyName("QuestionnaireId")] public int QuestionnaireId { get; set; }

    [JsonPropertyName("SortNumber")] public int SortNumber { get; set; }

    [JsonPropertyName("Title")] public string Title { get; set; } = string.Empty;

    [JsonPropertyName("CreateTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime CreateTime { get; set; }

    [JsonPropertyName("SubjectType")] public int SubjectType { get; set; }

    [JsonPropertyName("Score")] public double Score { get; set; }

    [JsonPropertyName("SequenceId")] public string SequenceId { get; set; } = string.Empty;

    [JsonPropertyName("IsSchool")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsSchool { get; set; }

    [JsonPropertyName("IsTeacher")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsTeacher { get; set; }

    [JsonPropertyName("Version")] public int Version { get; set; }

    [JsonPropertyName("IsSetAnswer")] public bool IsSetAnswer { get; set; }

    [JsonPropertyName("SubjectCategory")] public int SubjectCategory { get; set; }

    [JsonPropertyName("IsHadContrast")]
    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsHadContrast { get; set; }

    [JsonPropertyName("IsNotShow")] public bool IsNotShow { get; set; }

    [JsonPropertyName("ShowTextTitle")] public string ShowTextTitle { get; set; } = string.Empty;

    [JsonPropertyName("SubjectIdent")] public int SubjectIdent { get; set; }
}

public class ItemProps
{
    [JsonPropertyName("CustomAttr")] public CustomAttr CustomAttr { get; set; } = new();

    [JsonPropertyName("Items")] public ItemDetails Items { get; set; } = new();

    [JsonPropertyName("SubjectId")] public int SubjectId { get; set; }

    [JsonPropertyName("SubjectType")] public int SubjectType { get; set; }
}

public class CustomAttr
{
    [JsonPropertyName("DataOptions")] public DataOptions DataOptions { get; set; } = new();

    [JsonPropertyName("IsSetScore")] public bool IsSetScore { get; set; }

    [JsonPropertyName("IsAllowNull")] public bool IsAllowNull { get; set; }

    [JsonPropertyName("Remarks")] public string Remarks { get; set; } = string.Empty;
}

public class DataOptions
{
    [JsonPropertyName("Cols")] public int Cols { get; set; }

    [JsonPropertyName("MaxValue")] public int MaxValue { get; set; }

    [JsonPropertyName("MinValue")] public int MinValue { get; set; }

    [JsonPropertyName("MaxAnswerNumber")] public int MaxAnswerNumber { get; set; }

    [JsonPropertyName("MinAnswerNumber")] public int MinAnswerNumber { get; set; }

    [JsonPropertyName("Star")] public int Star { get; set; }

    [JsonPropertyName("FileNumber")] public int FileNumber { get; set; }

    [JsonPropertyName("FileType")] public int FileType { get; set; }

    [JsonPropertyName("MinTextNumber")] public int MinTextNumber { get; set; }

    [JsonPropertyName("HasPositiveTag")] public bool HasPositiveTag { get; set; }

    [JsonPropertyName("HasNegativeTag")] public bool HasNegativeTag { get; set; }

    [JsonPropertyName("HasAiComment")] public bool HasAiComment { get; set; }

    [JsonPropertyName("HasAiPolish")] public bool HasAiPolish { get; set; }
}

public class ItemDetails
{
    // [JsonPropertyName("Issues")] public List<object> Issues { get; set; } = new();

    [JsonPropertyName("Options")] public List<Option> Options { get; set; } = new();

    // [JsonPropertyName("Logics")] public List<object> Logics { get; set; } = new();

    [JsonPropertyName("OptionsCountRules")]
    public List<object> OptionsCountRules { get; set; } = new();

    [JsonPropertyName("OptionsExclusiveRules")]
    public List<object> OptionsExclusiveRules { get; set; } = new();

    [JsonPropertyName("OptionsScoreMethodFlag")]
    public int OptionsScoreMethodFlag { get; set; }
}

public class Option
{
    [JsonPropertyName("BandScore")] public double BandScore { get; set; }

    [JsonPropertyName("IsSetOptions")] public bool IsSetOptions { get; set; }

    [JsonPropertyName("IsExclusive")] public bool IsExclusive { get; set; }

    [JsonPropertyName("IsSetAnswer")] public bool IsSetAnswer { get; set; }

    [JsonPropertyName("IsHide")] public bool IsHide { get; set; }

    [JsonPropertyName("Id")] public int Id { get; set; }

    [JsonPropertyName("Title")] public string Title { get; set; } = string.Empty;

    [JsonPropertyName("SortNumber")] public int SortNumber { get; set; }
}