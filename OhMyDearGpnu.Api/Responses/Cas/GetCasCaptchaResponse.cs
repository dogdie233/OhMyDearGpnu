using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.Responses.Cas;

public class GetCasCaptchaResponse
{
    [JsonPropertyName("uid")][JsonConverter(typeof(CasGuidConverter))] public Guid Uid { get; set; }
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
    [JsonPropertyName("timeout")] public int Timeout { get; set; }
}