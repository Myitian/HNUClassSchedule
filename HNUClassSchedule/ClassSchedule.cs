using HNUClassSchedule.API;
using HNUClassSchedule.API.Login;
using HNUClassSchedule.API.Week;
using HNUClassSchedule.JsonUtils;
using HNUClassSchedule.RSAHelper;

#if NET6_0_OR_GREATER
using System.Net.Http.Json;
#endif

namespace HNUClassSchedule;

public class ClassSchedule : IDisposable
{
    public const string JWC_RootURL = "https://jwc.htu.edu.cn/";
    public const string JWC_LoginURL = JWC_RootURL + "dev-api/appapi/applogin";
    public const string JWC_GetTokenURL = JWC_RootURL + "dev-api/appapi/getIstoken";
    public const string JWC_TodayScheduleURL = JWC_RootURL + "dev-api/appapi/appqxkb/datagrkb";
    public const string JWC_WeekScheduleURL = JWC_RootURL + "dev-api/appapi/Studentkb/index";
    public const string JWC_FullScheduleURL = JWC_RootURL + "dev-api/appapi/Studentkb/data";
    public const string JWC_CalendarURL = JWC_RootURL + "new/desktop/getCalendar";
    public const string JWC_ReferrerURL = JWC_RootURL + "new/desktop";

    private readonly HttpClient http;
    private readonly CookieContainer cookies = new();
    private string? token;

    private static readonly RSAParameters rsaPublicKey = RSAKeyConverter.ParseKey("MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAKoR8mX0rGKLqzcWmOzbfj64K8ZIgOdHnzkXSOVOZbFu/TJhZ7rFAN+eaGkl3C4buccQd/EjEsj9ir7ijT7h96MCAwEAAQ==");
    private static readonly RSAParameters rsaPrivateKey = RSAKeyConverter.ParseKey("MIIBVAIBADANBgkqhkiG9w0BAQEFAASCAT4wggE6AgEAAkEAqhHyZfSsYourNxaY7Nt+PrgrxkiA50efORdI5U5lsW79MmFnusUA355oaSXcLhu5xxB38SMSyP2KvuKNPuH3owIDAQABAkAfoiLyL+Z4lf4Myxk6xUDgLaWGximj20CUf+5BKKnlrK+Ed8gAkM0HqoTt2UZwA5E2MzS4EI2gjfQhz5X28uqxAiEA3wNFxfrCZlSZHb0gn2zDpWowcSxQAgiCstxGUoOqlW8CIQDDOerGKH5OmCJ4Z21v+F25WaHYPxCFMvwxpcw99EcvDQIgIdhDTIqD2jfYjPTY8Jj3EDGPbH2HHuffvflECt3Ek60CIQCFRlCkHpi7hthhYhovyloRYsM+IS9h/0BzlEAuO0ktMQIgSPT3aFAgJYwKpqRYKlLDVcflZFCKY7u3UP8iWi1Qw0Y=");
    private static readonly RSA rsaPublic = RSA.Create();
    private static readonly RSA rsaPrivate = RSA.Create();
    private static readonly Uri jwcHtuEduCn = new(JWC_RootURL);

    static ClassSchedule()
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

    public string? JSessionID
    {
        get
        {
            Cookie? ssid = cookies.GetCookies(jwcHtuEduCn)["JSESSIONID"];
            return ssid is null || ssid.Expired ? null : ssid.Value;
        }
        set
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            cookies.Add(new Cookie("JSESSIONID", value, "/", jwcHtuEduCn.Host));
        }
    }

    public ClassSchedule(string? token = null, HttpMessageHandler? httpMessageHandler = null)
    {
        httpMessageHandler ??=
#if NETCOREAPP2_1_OR_GREATER
                new SocketsHttpHandler();
#else
                new HttpClientHandler();
#endif

        if (httpMessageHandler is HttpClientHandler hch)
        {
            cookies = hch.CookieContainer;
        }
#if NETCOREAPP2_1_OR_GREATER
        else if (httpMessageHandler is SocketsHttpHandler shh)
        {
            cookies = shh.CookieContainer;
        }
#endif
        else
        {
            throw new NotSupportedException();
        }

        http = new(httpMessageHandler);
        Token = token;
    }

    public async Task<(bool IsSuccess, LoginRoot? Response)> Login(string username, string password)
    {
        username = HttpUtility.JavaScriptStringEncode(username);
        password = Convert.ToBase64String(rsaPublic.Encrypt(Encoding.UTF8.GetBytes(password), RSAEncryptionPadding.Pkcs1));
        using StringContent content = new($"{{\"username\":\"{username}\",\"password\":\"{password}\"}}", Encoding.UTF8, "application/json");
        using HttpResponseMessage resp = await http.PostAsync(JWC_LoginURL, content);
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
        using HttpResponseMessage resp = await http.GetAsync(JWC_GetTokenURL);
        LoginRoot? r = await ReadJson<LoginRoot>(resp);
        return !string.IsNullOrWhiteSpace(r?.User?.Token);
    }

    public async Task<HttpResponseMessage> RequestTodayClassScheduleResp(int todaykb = 1)
    {
        using StringContent content = new($"{{\"todaykb\":\"{todaykb}\"}}", Encoding.UTF8, "application/json");
        return await http.PostAsync(JWC_TodayScheduleURL, content);
    }
    public async Task<CourseList?> RequestTodayClassSchedule(int todaykb = 1)
    {
        using HttpResponseMessage resp = await RequestTodayClassScheduleResp(todaykb);
        return await ReadJson<CourseList>(resp);
    }
    public async Task<string> RequestTodayClassScheduleJson(int todaykb = 1)
    {
        using HttpResponseMessage resp = await RequestTodayClassScheduleResp(todaykb);
        return await resp.Content.ReadAsStringAsync();
    }

    public async Task<HttpResponseMessage> RequestWeekClassScheduleResp(int? week = null, int? lesson = null)
    {
        using StringContent content = new($"{{\"zc\":\"{week}\",\"jc\":\"{lesson}\"}}", Encoding.UTF8, "application/json");
        return await http.PostAsync(JWC_WeekScheduleURL, content);
    }
    public async Task<WeekRoot?> RequestWeekClassSchedule(int? week = null, int? lesson = null)
    {
        using HttpResponseMessage resp = await RequestWeekClassScheduleResp(week, lesson);
        return await ReadJson<WeekRoot>(resp);
    }
    public async Task<string> RequestWeekClassScheduleJson(int? week = null, int? lesson = null)
    {
        using HttpResponseMessage resp = await RequestWeekClassScheduleResp(week, lesson);
        return await resp.Content.ReadAsStringAsync();
    }

    public async Task<HttpResponseMessage> RequestFullClassScheduleResp()
    {
        using StringContent content = new("{}", Encoding.UTF8, "application/json");
        return await http.PostAsync(JWC_FullScheduleURL, content);
    }
    public async Task<CourseList?> RequestFullClassSchedule()
    {
        using HttpResponseMessage resp = await RequestFullClassScheduleResp();
        return await ReadJson<CourseList>(resp);
    }
    public async Task<string> RequestFullClassScheduleJson()
    {
        using HttpResponseMessage resp = await RequestFullClassScheduleResp();
        return await resp.Content.ReadAsStringAsync();
    }

    public async Task<HttpResponseMessage> RequestFullClassScheduleResp(DateTime? start = null, DateTime? end = null)
    {
        List<KeyValuePair<string, string>> contentKVP = [];
        if (start.HasValue)
        {
            contentKVP.Add(new("d1", start.Value.ToString("yyyy-MM-dd HH:mm:ss")));
        }
        if (end.HasValue)
        {
            contentKVP.Add(new("d2", end.Value.ToString("yyyy-MM-dd HH:mm:ss")));
        }
        using FormUrlEncodedContent content = new(contentKVP);
        using HttpRequestMessage req = new(HttpMethod.Post, JWC_CalendarURL)
        {
            Content = content
        };
        req.Headers.Referrer = new(JWC_ReferrerURL);
        return await http.SendAsync(req);
    }
    public async Task<List<Lesson>?> RequestFullClassSchedule(DateTime? start = null, DateTime? end = null)
    {
        using HttpResponseMessage resp = await RequestFullClassScheduleResp(start, end);
        return await ReadJson<List<Lesson>>(resp);
    }
    public async Task<string> RequestFullClassScheduleJson(DateTime? start = null, DateTime? end = null)
    {
        using HttpResponseMessage resp = await RequestFullClassScheduleResp(start, end);
        return await resp.Content.ReadAsStringAsync();
    }

    private static async Task<T?> ReadJson<T>(HttpResponseMessage resp) where T : class
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
        http.Dispose();
    }
}
