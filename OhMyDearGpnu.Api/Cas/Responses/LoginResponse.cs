using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.Cas.Responses;

public class LoginResponse
{
    [JsonPropertyName("meta")] public MetaModel? Meta { get; set; }
    [JsonPropertyName("data")] public DataModel? Data { get; set; }

    [JsonPropertyName("ticket")] public string? Ticket { get; set; }
    [JsonPropertyName("tgt")] public string? Tgt { get; set; }

    public class MetaModel
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("statusCode")] public int StatusCode { get; set; }
        [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
    }

    public class DataModel
    {
        [JsonPropertyName("code")] public string Code { get; set; } = string.Empty;
        [JsonPropertyName("data")] public string Data { get; set; } = string.Empty;
    }
}