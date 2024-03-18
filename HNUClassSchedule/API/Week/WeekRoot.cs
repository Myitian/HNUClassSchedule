using HNUClassSchedule.JsonUtils;

namespace HNUClassSchedule.API.Week;

public class WeekRoot
{
    [JsonPropertyName("msg")]
    public string? Msg { get; set; }

    [JsonPropertyName("maxzc")]
    public string? MaxWeek { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("stillWeek")]
    public string? StillWeek { get; set; }

    [JsonPropertyName("zc")]
    public string? Week { get; set; }

    [JsonPropertyName("curDay")]
    public string? CurrentDay { get; set; }

    [JsonPropertyName("minzc")]
    public string? MinWeek { get; set; }

    [JsonConverter(typeof(StringJsonContentJsonConverter<List<Dictionary<string, List<Lesson>>>>))]
    [JsonPropertyName("kbList")]
    public List<Dictionary<string, List<Lesson>>>? ClassSchedule { get; set; }

    [JsonPropertyName("issj")]
    public bool HasTime { get; set; }

    [JsonPropertyName("xnxqdm")]
    public string? AcademicYearAndSemester { get; set; }

    [JsonPropertyName("isOpen")]
    public bool Unknown_IsOpen { get; set; }

    [JsonPropertyName("xldata")]
    public List<WeekdayInfo>? ColumnIndexes { get; set; }

    [JsonPropertyName("nodelist")]
    public List<RowIndex>? RowIndexes { get; set; }

    [JsonPropertyName("modelist")]
    public List<LessonNumberInfo>? LessonNumbers { get; set; }

    [JsonPropertyName("rq")]
    public string? Date { get; set; }
}
