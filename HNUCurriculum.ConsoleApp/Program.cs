using HNUCurriculum.API;
using HNUCurriculum.API.Login;

namespace HNUCurriculum.ConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        Translation tr = new();
        Curriculum curriculum = await CreateCurriculum(ReadNotNullString("用户名："), true, null, tr);
        // var today = await curriculum.RequestTodayCurriculum();
        // var week = await curriculum.RequestWeekCurriculum();
        var all = await curriculum.RequestAllCurriculum();
        if (all?.Lessons is null)
        {
            Message(tr[all?.Msg]);
            return;
        }
        all.Lessons.Sort();
        Message("三天内课表：");
        DateTime date = DateTime.Now.Date;
        foreach (var item in all.Lessons.Where(L => L.StartDate >= date && L.StartDate - date <= TimeSpan.FromDays(3)))
        {
            Console.WriteLine("========================================");
            Console.WriteLine(item);
        }
        Console.WriteLine("========================================");
    }

    public static async Task<Curriculum> CreateCurriculum(string username, bool useCache = true, HttpClient? http = null, Translation? translation = null)
    {
        Curriculum curriculum = new(httpClient: http);
        if (useCache && File.Exists(username))
        {
            Info("检测到 Token 缓存");
            string token = File.ReadAllText(username);
            curriculum.Token = token;
            if (await curriculum.VerifyLoginStatus())
            {
                Info("缓存有效");
                return curriculum;
            }
            Info("缓存无效，需要密码");
        }
        (bool IsSuccess, LoginRoot? Response) loginResp;
        translation ??= new Translation();
        while (!(loginResp = await curriculum.Login(username, ReadPassword("密码："))).IsSuccess)
        {
            Console.WriteLine(translation[loginResp.Response?.Msg]);
        }
        if (useCache)
        {
            File.WriteAllText(username, curriculum.Token);
        }
        return curriculum;
    }

    public static string ReadPassword(string prompt = "")
    {
        Console.Write(prompt);
        Stack<char> pwd = new();
        ConsoleKeyInfo key = Console.ReadKey(true);
        while (key.Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace)
            {
                pwd.Pop();
            }
            else
            {
                pwd.Push(key.KeyChar);
            }
            key = Console.ReadKey(true);
        }
        Console.WriteLine();
        return new string(pwd.Reverse().ToArray());
    }

    public static string ReadNotNullString(string prompt = "")
    {
        Console.Write(prompt);
        string? read;
        while ((read = Console.ReadLine()) is null) ;
        return read;
    }

    public static void Info(string message)
    {
        Console.Write("** ");
        Console.WriteLine(message);
    }
    public static void Message(string message)
    {
        Console.WriteLine(message);
    }
}
