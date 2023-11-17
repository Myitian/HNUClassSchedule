using System.Text.Json.Serialization;

namespace HNUCurriculum.API.Login;

public class LoginRoot
{
    [JsonPropertyName("msg")]
    public string? Msg { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("twofactorAuth")]
    public string? TwoFactorAuth { get; set; }

    [JsonPropertyName("appid")]
    public string? AppID { get; set; }

    [JsonPropertyName("isbind")]
    public string? IsBound { get; set; }

    [JsonPropertyName("weakPass")]
    public bool IsWeakPassword { get; set; }

    [JsonPropertyName("initPw")]
    public bool IsInitialPassword { get; set; }

    [JsonPropertyName("user")]
    public User? User { get; set; }
}
