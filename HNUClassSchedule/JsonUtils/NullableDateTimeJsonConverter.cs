namespace HNUClassSchedule.JsonUtils;

public class NullableDateTimeJsonConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.TryParse(reader.GetString(), out DateTime dateTime) ? dateTime : null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
