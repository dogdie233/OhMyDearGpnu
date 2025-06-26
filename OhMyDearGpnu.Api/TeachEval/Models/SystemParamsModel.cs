using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public record SystemParamsModel
{
    public int DegreeLevel { get; set; }
    public required string ApiName { get; set; }

    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime ClientTime { get; set; }

    public required string ClientId { get; set; }
    public required string RequestOriginPageAddress { get; set; }

    public string? Semester { get; set; }
    public string? Token { get; set; }
    public string? UserCode { get; set; }
    public string? UniversityCode { get; set; }

    public PagedReqModel? PageContext { get; set; }
}