using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.StuAff.PartTimeJob.Models;

public class MonthlySalaryModel
{
    [JsonPropertyName("GZLID")] public string RegistrationId { get; set; } = string.Empty;
    [JsonPropertyName("JSJE")] public decimal ConvertedSalary { get; set; }

    [JsonPropertyName("NY")]
    [JsonConverter(typeof(DateOnlyYearMonthConverter))]
    public DateOnly Date { get; set; }

    [JsonPropertyName("ZGL")] public int WorkHours { get; set; }
    [JsonPropertyName("XCJE")] public decimal RealSalary { get; set; }
    [JsonPropertyName("ROW_ID")] public int RowId { get; set; }
}