using System.Text.Json;
using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.Serializer.Json;

public class TeachEvalDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrEmpty(str))
            return default;

        return DateTime.TryParse(str, out var result) ? result : default;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var str = value.ToString("yyyy-MM-dd HH:mm:ss");
        writer.WriteStringValue(str);
    }
}