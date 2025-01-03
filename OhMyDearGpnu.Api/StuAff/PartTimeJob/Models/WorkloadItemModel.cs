using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.StuAff.PartTimeJob.Models;

public class WorkloadItemModel
{
    [JsonPropertyName("GWXXID")] public Guid JobId { get; set; }

    [JsonPropertyName("GZRQ")] public DateOnly WorkDate { get; set; }

    [JsonPropertyName("KSSJ")]
    [JsonConverter(typeof(TimeOnlyHourMinuteConverter))]
    public TimeOnly StartTime { get; set; }

    [JsonPropertyName("JSSJ")]
    [JsonConverter(typeof(TimeOnlyHourMinuteConverter))]
    public TimeOnly EndTime { get; set; }

    [JsonPropertyName("GZSC")] public int WorkHours { get; set; }

    [JsonPropertyName("GZLID")] public Guid RegistrationId { get; set; }

    [JsonPropertyName("XSID")] public string StudentId { get; set; } = string.Empty;

    [JsonPropertyName("ROW_ID")] public int RowId { get; set; }
}