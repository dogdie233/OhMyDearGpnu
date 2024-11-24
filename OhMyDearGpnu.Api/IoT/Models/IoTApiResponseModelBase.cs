using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.IoT.Models;

public class IoTApiResponseModelBase<T> where T : class
{
    [JsonPropertyName("code")] public int Code { get; set; }
    [JsonPropertyName("data")] public T Data { get; set; } = default!;
    [JsonPropertyName("msg")] public string Message { get; set; } = string.Empty;
}