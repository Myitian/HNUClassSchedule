using System.Text.Json.Serialization;

namespace HNUCurriculum.API.Week;

public class LessonNumberInfo
{
    [JsonPropertyName("jcdm")]
    public string? Name1 { get; set; }

    [JsonPropertyName("jcdm2")]
    public string? Name2 { get; set; }

    [JsonPropertyName("szdm")]
    public int Number { get; set; }

    [JsonPropertyName("sjmc")]
    public string? Name { get; set; }

    /// <summary>fzmc</summary>
    [JsonPropertyName("fzmc")]
    public string? Unknown_0 { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
