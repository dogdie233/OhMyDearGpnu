using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.StuAff.Models;

public class JsonResponseModel<T>
{
    [JsonPropertyName("code")] public int Code { get; set; }
    [JsonPropertyName("data")] public T Data { get; set; } = default!;
    [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
}