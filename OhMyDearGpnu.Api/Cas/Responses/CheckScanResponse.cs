using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static OhMyDearGpnu.Api.Cas.Responses.LoginResponse;

namespace OhMyDearGpnu.Api.Cas.Responses;

public class CheckScanResponse
{
    [JsonPropertyName("data")] public CheckScanDataModel? Data { get; set; }
    [JsonPropertyName("meta")] public MetaModel Meta { get; set; } = null!;
}

public class CheckScanDataModel
{
    [JsonPropertyName("passWord")] public string Password { get; set; } = "";
    [JsonPropertyName("userName")] public string UserName { get; set; } = "";
    [JsonPropertyName("service")] public string Service { get; set; } = "";
}