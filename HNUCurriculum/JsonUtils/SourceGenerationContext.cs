using HNUCurriculum.API;
using HNUCurriculum.API.Login;
using HNUCurriculum.API.Week;
using System.Text.Json.Serialization;

namespace HNUCurriculum.JsonUtils
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(CourseList))]
    [JsonSerializable(typeof(WeekRoot))]
    [JsonSerializable(typeof(LoginRoot))]
    internal partial class SourceGenerationContext : JsonSerializerContext
    {
    }
}
