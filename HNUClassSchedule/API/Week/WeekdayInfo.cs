namespace HNUClassSchedule.API.Week;

public class WeekdayInfo
{
    [JsonPropertyName("xqmc1")]
    public string? Name1 { get; set; }

    [JsonPropertyName("xnxqdm")]
    public string? AcademicYearAndSemester { get; set; }

    [JsonPropertyName("xqmc")]
    public string? Name { get; set; }

    [JsonPropertyName("zc")]
    public string? Week { get; set; }

    [JsonPropertyName("mxrq")]
    public string? Date { get; set; }
}
