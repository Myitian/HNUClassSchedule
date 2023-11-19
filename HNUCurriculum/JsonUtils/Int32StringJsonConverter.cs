using System.Text.Json;
using System.Text.Json.Serialization;

namespace HNUCurriculum.JsonUtils
{
    public class Int32StringJsonConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (int.TryParse(reader.GetString(), out int result))
            {
                return result;
            }
            return 0;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
