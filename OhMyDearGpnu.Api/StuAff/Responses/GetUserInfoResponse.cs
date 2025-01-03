using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.StuAff.Models;

// ReSharper disable ClassNeverInstantiated.Global

namespace OhMyDearGpnu.Api.StuAff.Responses;

public class GetUserInfoResponse
{
    [JsonPropertyName("meta")]
    [JsonRequired]
    public MetaModel Meta { get; set; } = null!;

    [JsonPropertyName("data")] public UserInfoModel? Data { get; set; }

    public class MetaModel
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("statusCode")] public int StatusCode { get; set; }
        [JsonPropertyName("message")] public string? Message { get; set; }
    }
}