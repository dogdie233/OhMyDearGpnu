using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.AcaAff.Responses;

public class GetLoginPublicKeyData
{
    [JsonPropertyName("exponent")] public string Exponent { get; set; } = null!;
    [JsonPropertyName("modulus")] public string Modulus { get; set; } = null!;
}