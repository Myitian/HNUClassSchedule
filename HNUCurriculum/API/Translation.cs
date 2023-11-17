using System;
using System.Collections.Generic;
using System.Text;

namespace HNUCurriculum.API
{
    public class Translation
    {
        protected Dictionary<string, Dictionary<string, string>> TranslationMappings = new()
        {
            { "zh_CN", new() {
                { "app_retrun_success_public", "操作成功" },
                { "app_retrun_error_applogin", "您的帐号或身份证号或密码不正确" }
            } }
        };
        protected HashSet<string> Languages = new() { "zh_CN", "zh_HK", "en_US", "ja_JP" };
        protected string FallbackLanguage = "zh_CN";
        private string language = "zh_CN";

        public string Language
        {
            get => language;
            set
            {
                if(Languages.Contains(value))
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
                if (TranslationMappings.TryGetValue(Language, out Dictionary<string, string>? mapping)
                    && mapping.TryGetValue(key, out string? value))
                    return value;
                if (TranslationMappings.TryGetValue(FallbackLanguage, out mapping)
                    && mapping.TryGetValue(key, out value))
                    return value;
                return key;
            }
        }
    }
}
