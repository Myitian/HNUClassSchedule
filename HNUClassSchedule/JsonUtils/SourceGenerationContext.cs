using HNUClassSchedule.API;
using HNUClassSchedule.API.Login;
using HNUClassSchedule.API.Week;

namespace HNUClassSchedule.JsonUtils;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(CourseList))]
[JsonSerializable(typeof(WeekRoot))]
[JsonSerializable(typeof(LoginRoot))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
