namespace HNUClassSchedule.JsonUtils;

public class StringJsonContentJsonConverter<T> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? inner = reader.GetString();
        if (inner is null)
            return default;
        if (JsonSerializer.Deserialize(inner, typeof(T), SourceGenerationContext.Default) is T t)
            return t;
        return default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(JsonSerializer.Serialize(value, typeof(T), SourceGenerationContext.Default));
    }
}
