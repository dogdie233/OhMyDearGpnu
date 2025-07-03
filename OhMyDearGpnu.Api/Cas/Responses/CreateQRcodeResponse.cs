using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.Cas.Responses;

public class CreateQRcodeResponse
{
    [JsonPropertyName("uuid")] public string Uuid { get; set; } = null!;
    
    [JsonConverter(typeof(StringImageConverter))]
    [JsonPropertyName("content")] public byte[] Content { get; set; } = null!;
}
