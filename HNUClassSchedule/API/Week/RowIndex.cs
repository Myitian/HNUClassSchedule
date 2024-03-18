namespace HNUClassSchedule.API.Week;

public class RowIndex
{
    [JsonPropertyName("rownum")]
    public int RowNum { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
