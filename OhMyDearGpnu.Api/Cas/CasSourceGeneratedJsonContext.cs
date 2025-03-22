using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Cas.Responses;

namespace OhMyDearGpnu.Api.Cas;

[JsonSerializable(typeof(GetCasCaptchaResponse))]
[JsonSerializable(typeof(LoginResponse))]
public partial class CasSourceGeneratedJsonContext : JsonSerializerContext { }