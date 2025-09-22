using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.Serializer.Json;

public class NumberStringConverter<T> : JsonConverter<T> where T : INumber<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString() ?? throw new JsonException("String value is null.");
        return T.TryParse(stringValue, null, out var result) ? result : throw new JsonException($"Invalid number format: {stringValue}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}