namespace HNUClassSchedule.API;

public class Translation
{
    protected Dictionary<string, Dictionary<string, string>> TranslationMappings = new()
    {
        { "zh-CN", new() {
            { "app_retrun_success_public", "操作成功" },
            { "app_retrun_error_applogin", "您的帐号或身份证号或密码不正确" }
        } },
        { "zh-TW", new() {
            { "app_retrun_success_public", "操作成功" },
            { "app_retrun_error_applogin", "您的帳號或身份證號或密碼不正確" }
        } },
        { "en-US", new() {
            { "app_retrun_success_public", "Operation successful" },
            { "app_retrun_error_applogin", "Your account or ID number number or password is incorrect" }
        } },
        { "ja-JP", new() {
            { "app_retrun_success_public", "操作が成功しました" },
            { "app_retrun_error_applogin", "アカウントまたはID番号またはパスワードが正しくありません" }
        } },
        { "ko-KR", new() {
            { "app_retrun_success_public", "작업 성공" },
            { "app_retrun_error_applogin", "계정 또는 주민등록번호 또는 암호가 잘못되었습니다." }
        } }
    };
    protected HashSet<string> Languages = ["zh-CN", "zh-TW", "en-US", "ja-JP", "ko-KR"];
    protected string FallbackLanguage = "zh-CN";
    private string language = "zh-CN";

    public string Language
    {
        get => language;
        set
        {
            if (Languages.Contains(value))
                language = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value));
        }
    }

    public string this[string? key]
    {
        get
        {
            if (key is null)
                return "";
            if (TranslationMappings.TryGetValue(language, out Dictionary<string, string>? mapping)
                && mapping.TryGetValue(key, out string? value))
                return value;
            if (TranslationMappings.TryGetValue(FallbackLanguage, out mapping)
                && mapping.TryGetValue(key, out value))
                return value;
            return key;
        }
    }
}
