using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.StuAff.PartTimeJob.Models;

public class MyJobModel
{
    [JsonPropertyName("SFXSMC")] public string ApplicationChannel { get; set; } = string.Empty;

    [JsonPropertyName("JSSJ")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("YHKH")] public string BankCardNumber { get; set; } = string.Empty;
    [JsonPropertyName("GWLB")] public string JobCategory { get; set; } = string.Empty;
    [JsonPropertyName("KHH")] public string BankName { get; set; } = string.Empty;
    [JsonPropertyName("RZGWID")] public Guid RZGWID { get; set; }
    [JsonPropertyName("GWXXID")] public Guid JobId { get; set; }
    [JsonPropertyName("GWXZMC")] public string JobNatureName { get; set; } = string.Empty;

    [JsonPropertyName("SQSJ")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ApplicationTime { get; set; }

    // "SFM": "1", nullable
    [JsonPropertyName("SFM")] public string? SFM { get; set; }
    [JsonPropertyName("YXMC")] public string CollegeName { get; set; } = string.Empty;
    [JsonPropertyName("KSSJ")] public string StartTime { get; set; } = string.Empty;
    [JsonPropertyName("XNMC")] public string WorkYearName { get; set; } = string.Empty;

    // "RZZTM": "1", nullable
    [JsonPropertyName("RZZTM")] public string? RZZTM { get; set; }
    [JsonPropertyName("RZXQ")] public string WorkTermName { get; set; } = string.Empty;
    [JsonPropertyName("GWMC")] public string JobName { get; set; } = string.Empty;

    // "SFXS": "1",
    [JsonPropertyName("SFXS")] public string SFXS { get; set; } = string.Empty;
    [JsonPropertyName("XB")] public string Sex { get; set; } = string.Empty;

    // "BPMJDID": "T10001", nullable
    [JsonPropertyName("BPMJDID")] public string? BPMJDID { get; set; }
    [JsonPropertyName("GZSJ")] public string WorkTime { get; set; } = string.Empty;
    [JsonPropertyName("LCSLID")] public string DocId { get; set; } = string.Empty;
    [JsonPropertyName("ROW_ID")] public int RowId { get; set; }
    [JsonPropertyName("XH")] public string UserId { get; set; } = string.Empty;

    // "SPZT": "2",
    [JsonPropertyName("SPZT")] public string SPZT { get; set; } = string.Empty;
    [JsonPropertyName("LCID")] public Guid ProcessId { get; set; }
    [JsonPropertyName("XM")] public string StudentName { get; set; } = string.Empty;
    [JsonPropertyName("SQSJMC")] public string ApplicationTimeName { get; set; } = string.Empty;
    [JsonPropertyName("GWLBM")] public string JobCategoryM { get; set; } = string.Empty;
    [JsonPropertyName("BMMC1")] public string DepartmentName1 { get; set; } = string.Empty;
    [JsonPropertyName("LXFS")] public string PhoneNumber { get; set; } = string.Empty;
    [JsonPropertyName("ZT")] public string Status { get; set; } = string.Empty;
    [JsonPropertyName("BMMC")] public string DepartmentName { get; set; } = string.Empty;
    [JsonPropertyName("XSID")] public string StudentId { get; set; } = string.Empty;
    [JsonPropertyName("XCZE")] public float Salary { get; set; }
}