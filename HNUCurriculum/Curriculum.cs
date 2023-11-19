using HNUCurriculum.API;
using HNUCurriculum.API.Week;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using HNUCurriculum.API.Login;
using HNUCurriculum.RSAHelper;
using System.Text.Json.Serialization;
using HNUCurriculum.JsonUtils;
using System.Text.Json;

#if NET6_0_OR_GREATER
using System.Net.Http.Json;
#endif

namespace HNUCurriculum;

public class Curriculum : IDisposable
{
    private readonly HttpClient http;
    private string? token;

    private static RSAParameters rsaPublicKey = RSAKeyConverter.ParseKey("MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAKoR8mX0rGKLqzcWmOzbfj64K8ZIgOdHnzkXSOVOZbFu/TJhZ7rFAN+eaGkl3C4buccQd/EjEsj9ir7ijT7h96MCAwEAAQ==");
    private static RSAParameters rsaPrivateKey = RSAKeyConverter.ParseKey("MIIBVAIBADANBgkqhkiG9w0BAQEFAASCAT4wggE6AgEAAkEAqhHyZfSsYourNxaY7Nt+PrgrxkiA50efORdI5U5lsW79MmFnusUA355oaSXcLhu5xxB38SMSyP2KvuKNPuH3owIDAQABAkAfoiLyL+Z4lf4Myxk6xUDgLaWGximj20CUf+5BKKnlrK+Ed8gAkM0HqoTt2UZwA5E2MzS4EI2gjfQhz5X28uqxAiEA3wNFxfrCZlSZHb0gn2zDpWowcSxQAgiCstxGUoOqlW8CIQDDOerGKH5OmCJ4Z21v+F25WaHYPxCFMvwxpcw99EcvDQIgIdhDTIqD2jfYjPTY8Jj3EDGPbH2HHuffvflECt3Ek60CIQCFRlCkHpi7hthhYhovyloRYsM+IS9h/0BzlEAuO0ktMQIgSPT3aFAgJYwKpqRYKlLDVcflZFCKY7u3UP8iWi1Qw0Y=");
    private static RSA rsaPublic = RSA.Create();
    private static RSA rsaPrivate = RSA.Create();

    static Curriculum()
    {
        rsaPublic.ImportParameters(rsaPublicKey);
        rsaPrivate.ImportParameters(rsaPrivateKey);
    }

    public string? Token
    {
        get => token;
        set
        {
            http.DefaultRequestHeaders.Remove("token");
            token = value;
            if (value is not null)
            {
                http.DefaultRequestHeaders.Add("token", value);
            }
        }
    }

    public Curriculum(string? token = null, HttpClient? httpClient = null)
    {
        http = httpClient ?? new();
        Token = token;
    }

    public async Task<(bool IsSuccess, LoginRoot? Response)> Login(string username, string password)
    {
        username = HttpUtility.JavaScriptStringEncode(username);
        password = Convert.ToBase64String(rsaPublic.Encrypt(Encoding.UTF8.GetBytes(password), RSAEncryptionPadding.Pkcs1));
        using StringContent content = new($"{{\"username\":\"{username}\",\"password\":\"{password}\"}}", Encoding.UTF8, "application/json");
        using HttpResponseMessage resp = await http.PostAsync("https://jwc.htu.edu.cn/dev-api/appapi/applogin", content);
        LoginRoot? r = await ReadJson<LoginRoot>(resp);
        if (r?.User?.Token is not null)
        {
            Token = r.User.Token;
            return (true, r);
        }
        return (false, r);
    }

    public async Task<bool> VerifyLoginStatus()
    {
        using HttpResponseMessage resp = await http.GetAsync("https://jwc.htu.edu.cn/dev-api/appapi/getIstoken");
        LoginRoot? r = await ReadJson<LoginRoot>(resp);
        return !string.IsNullOrWhiteSpace(r?.User?.Token);
    }

    public async Task<CourseList?> RequestTodayCurriculum(int todaykb = 1)
    {
        using StringContent content = new($"{{\"todaykb\":\"{todaykb}\"}}", Encoding.UTF8, "application/json");
        using HttpResponseMessage resp = await http.PostAsync("https://jwc.htu.edu.cn/dev-api/appapi/appqxkb/datagrkb", content);
        return await ReadJson<CourseList>(resp);
    }

    public async Task<WeekRoot?> RequestWeekCurriculum(int? week = null, int? lesson = null)
    {
        using StringContent content = new($"{{\"zc\":\"{week}\",\"jc\":\"{lesson}\"}}", Encoding.UTF8, "application/json");
        using HttpResponseMessage resp = await http.PostAsync("https://jwc.htu.edu.cn/dev-api/appapi/Studentkb/index", content);
        return await ReadJson<WeekRoot>(resp);
    }

    public async Task<CourseList?> RequestAllCurriculum()
    {
        using StringContent content = new($"{{}}", Encoding.UTF8, "application/json");
        using HttpResponseMessage resp = await http.PostAsync("https://jwc.htu.edu.cn/dev-api/appapi/Studentkb/data", content);
        return await ReadJson<CourseList>(resp);
    }

    private async Task<T?> ReadJson<T>(HttpResponseMessage resp) where T : class
    {
#if NET6_0_OR_GREATER
        return await resp.Content.ReadFromJsonAsync(typeof(T), SourceGenerationContext.Default) as T;
#else
        using Stream s = await resp.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync(s, typeof(T), SourceGenerationContext.Default) as T;
#endif
    }

    public void Dispose()
    {
        rsaPublic.Dispose();
        rsaPrivate.Dispose();
    }
}
