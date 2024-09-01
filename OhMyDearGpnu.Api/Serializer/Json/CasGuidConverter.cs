using System.Text.Json;
using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.Serializer.Json;

public class CasGuidConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var guid = reader.GetString();
        if (guid is null)
            throw new JsonException();
        return Guid.Parse(guid);
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("N"));
    }
}