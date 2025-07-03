using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Serializer.Json;

public class StringImageConverter : JsonConverter<byte[]>
{
    public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value is not { Length: > 22 })
            return null;
        return Utils.DecodeBase64(value.AsSpan()[22..]);
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        if (value == null) { writer.WriteNullValue(); return; }
        var base64string = "data:image/png;base64," + Convert.ToBase64String(value);
        writer.WriteStringValue(base64string);
    }
}
