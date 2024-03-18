using HNUClassSchedule;
using HNUClassSchedule.API;
using HNUClassSchedule.API.Login;


Dictionary<string, string?> argPairs = [];
string? username = null, password = null;
for (int i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "-p":
        case "-pwd":
        case "-password":
            i++;
            if (i >= args.Length)
                goto SuperBreak;
            username = args[i];
            break;
        case "-u":
        case "-usr":
        case "-user":
        case "-username":
            i++;
            if (i >= args.Length)
                goto SuperBreak;
            password = args[i];
            break;
    }
}
SuperBreak:
Translation tr = new();
var classSchedule = await CreateClassSchedule(username, password, translation: tr);
var today = await classSchedule.RequestTodayClassSchedule();
var week = await classSchedule.RequestWeekClassSchedule();
var all = await classSchedule.RequestFullClassSchedule();
DateTime date = DateTime.Now.Date;

Dictionary<Lesson, BoxedInt32> count = new(new LessonIDEqualityComparer());

Console.WriteLine("----------------------------------------------------------------");
if (week?.ClassSchedule is null)
{
    Message(tr[week?.Msg]);
    return;
}
else
{
    Message("本周课表：");
    var wl = new HashSet<Lesson>(week.ClassSchedule.SelectMany(x => x.Values).SelectMany(x => x)).ToList();
    wl.Sort();
    foreach (var item in wl)
    {
        Console.WriteLine("========================================");
        Console.WriteLine(item);
    }
    Console.WriteLine("========================================");
    Console.WriteLine();
    Console.WriteLine("----------------------------------------------------------------");
}
if (all?.Lessons is null)
{
    Message(tr[all?.Msg]);
}
else
{
    all.Lessons.Sort();
    Message("三天内课表：");
    DateTime day3 = date + TimeSpan.FromDays(3);
    foreach (var item in all.Lessons)
    {
        if (item.StartDate >= date && item.StartDate <= day3)
        {
            Console.WriteLine("========================================");
            Console.WriteLine(item);
        }
        if (count.TryGetValue(item, out BoxedInt32? num))
        {
            ++num;
        }
        else
        {
            count[item] = 1;
        }
    }
    Console.WriteLine("========================================");
    Console.WriteLine();
    Console.WriteLine("----------------------------------------------------------------");
}
if (today?.Lessons is null)
{
    Message(tr[today?.Msg]);
}
else
{
    today.Lessons.Sort();
    Message("今日课表：");
    foreach (var item in today.Lessons)
    {
        Console.WriteLine("========================================");
        Console.WriteLine(item);
    }
    Console.WriteLine("========================================");
    Console.WriteLine();
    Console.WriteLine("----------------------------------------------------------------");
}
if (count.Count > 0)
{
    Message("课程数量：");
    Console.WriteLine("========================================");
    foreach (var item in count)
    {
        Console.WriteLine($"{item.Key.CourseName}：{item.Value.Value}");
    }
    Console.WriteLine("========================================");
}

static async Task<ClassSchedule> CreateClassSchedule(
    string? username = null,
    string? password = null,
    bool useCache = true,
    HttpMessageHandler? httpMessageHandler = null,
    Translation? translation = null)
{
    username ??= ReadNotNullString("用户名：");
    ClassSchedule classSchedule = new(httpMessageHandler: httpMessageHandler);
    if (useCache && File.Exists(username))
    {
        Info("检测到 Token 缓存");
        string token = File.ReadAllText(username);
        classSchedule.Token = token;
        if (await classSchedule.VerifyLoginStatus())
        {
            Info("缓存有效");
            return classSchedule;
        }
        Info("缓存无效，需要密码");
    }
    (bool IsSuccess, LoginRoot? Response) loginResp;
    translation ??= new Translation();
    while (!(loginResp = await classSchedule.Login(username, password ?? ReadPassword("密码："))).IsSuccess)
    {
        Console.WriteLine(translation[loginResp.Response?.Msg]);
    }
    if (useCache)
    {
        File.WriteAllText(username, classSchedule.Token);
    }
    return classSchedule;
}

static string ReadPassword(string prompt = "")
{
    Console.Write(prompt);
    Stack<char> pwd = new();
    ConsoleKeyInfo key = Console.ReadKey(true);
    while (key.Key != ConsoleKey.Enter)
    {
        if (key.Key == ConsoleKey.Backspace && pwd.Count > 0)
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

static string ReadNotNullString(string prompt = "")
{
    Console.Write(prompt);
    string? read;
    while ((read = Console.ReadLine()) is null) ;
    return read;
}

static void Info(string message)
{
    Console.WriteLine("** " + message);
}
static void Message(string message)
{
    Console.WriteLine("# " + message);
}

class BoxedInt32(int value)
{
    public int Value = value;

    public static BoxedInt32 operator --(BoxedInt32 value)
    {
        value.Value--;
        return value;
    }
    public static BoxedInt32 operator ++(BoxedInt32 value)
    {
        value.Value++;
        return value;
    }
    public static implicit operator int(BoxedInt32 value) => value.Value;
    public static implicit operator BoxedInt32(int value) => new(value);
}

class LessonIDEqualityComparer : IEqualityComparer<Lesson>
{
    public bool Equals(Lesson? x, Lesson? y)
    {
        return x?.CourseNumber == y?.CourseNumber;
    }

    public int GetHashCode(Lesson obj)
    {
        return obj?.CourseNumber?.GetHashCode() ?? 0;
    }
}