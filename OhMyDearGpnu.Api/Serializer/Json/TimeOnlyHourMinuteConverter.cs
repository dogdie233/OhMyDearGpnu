using System.Text.Json;
using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.Serializer.Json;

public class TimeOnlyHourMinuteConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException();

        if (TimeOnly.TryParse(reader.GetString(), out var time))
            return time;

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("HH:mm"));
    }
}