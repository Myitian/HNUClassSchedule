namespace HNUClassSchedule.API.Login;

public class User
{
    [JsonPropertyName("lastLoginTime")]
    public string? LastLoginTime { get; set; }

    [JsonPropertyName("loginIp")]
    public string? LoginIP { get; set; }

    [JsonPropertyName("loginTime")]
    public string? LoginTime { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }
}
