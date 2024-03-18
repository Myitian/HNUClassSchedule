namespace HNUClassSchedule.API;

public class CourseList
{
    [JsonPropertyName("msg")]
    public string? Msg { get; set; }

    [JsonPropertyName("issj")]
    public bool HasTime { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("isOpen")]
    public bool IsOpen { get; set; }

    [JsonPropertyName("kbList")]
    public List<Lesson>? Lessons { get; set; }
}
