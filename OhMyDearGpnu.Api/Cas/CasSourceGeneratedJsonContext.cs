using System.Text.Json.Serialization;
using OhMyDearGpnu.Api.Cas.Requests;
using OhMyDearGpnu.Api.Cas.Responses;

namespace OhMyDearGpnu.Api.Cas;

[JsonSerializable(typeof(GetCasCaptchaResponse))]
[JsonSerializable(typeof(LoginResponse))]
[JsonSerializable(typeof(CheckScanRequest))]
[JsonSerializable(typeof(CheckScanResponse))]
[JsonSerializable(typeof(CreateQRcodeRequest))]
[JsonSerializable(typeof(CreateQRcodeResponse))]
public partial class CasSourceGeneratedJsonContext : JsonSerializerContext { }