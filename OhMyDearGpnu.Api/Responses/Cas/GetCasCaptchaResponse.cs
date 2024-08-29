using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.Responses.Cas;

public class GetCasCaptchaResponse
{
    [JsonPropertyName("uid")] public Guid Uid { get; set; }
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
    [JsonPropertyName("timeout")] public int Timeout { get; set; }
}