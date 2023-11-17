using HNUCurriculum.JsonConverters;
using System.Text;
using System.Text.Json.Serialization;

namespace HNUCurriculum.API;

public class Lesson : IComparable<Lesson>
{
    /// <summary>分组</summary>
    [JsonPropertyName("flfzmc")]
    public string? Grouping { get; set; }

    /// <summary>考核方式</summary>
    [JsonPropertyName("khfsmc")]
    public string? AssessmentMethod { get; set; }

    /// <summary>起始课序号</summary>
    [JsonPropertyName("ps")]
    public string? StartLessonNumber { get; set; }

    /// <summary>排课人数</summary>
    [JsonPropertyName("pkrs")]
    public int ScheduledClassSize { get; set; }

    /// <summary>教学内容</summary>
    [JsonPropertyName("sknrjj")]
    public string? TeachingContent { get; set; }

    /// <summary>经纬度</summary>
    [JsonPropertyName("lngandlat")]
    public string? LongitudeAndLatitude { get; set; }

    /// <summary>座位号</summary>
    [JsonPropertyName("zwh")]
    public int Seat { get; set; }

    /// <summary>教学环节</summary>
    [JsonPropertyName("jxhjmc")]
    public string? TeachingEnvironment { get; set; }

    /// <summary>起始时间</summary>
    [JsonConverter(typeof(NullableDateTimeJsonConverter))]
    [JsonPropertyName("qssj")]
    public DateTime? StartTime { get; set; }

    /// <summary>课程类型</summary>
    [JsonPropertyName("lx")]
    public string? TaskType { get; set; }

    /// <summary>学年学期</summary>
    [JsonPropertyName("xnxqmc")]
    public string? AcademicYearAndSemester { get; set; }

    /// <summary>教学班代码</summary>
    [JsonPropertyName("jxbdm")]
    public string? ClassCode { get; set; }

    /// <summary>结束时间</summary>
    [JsonConverter(typeof(NullableDateTimeJsonConverter))]
    [JsonPropertyName("jssj")]
    public DateTime? EndTime { get; set; }

    /// <summary>起始日期</summary>
    [JsonConverter(typeof(NullableDateTimeJsonConverter))]
    [JsonPropertyName("qsrq")]
    public DateTime? StartDate { get; set; }

    /// <summary>教学班人数</summary>
    [JsonPropertyName("jxbrs")]
    public int ClassSize { get; set; }

    /// <summary>备注</summary>
    [JsonPropertyName("bz")]
    public string? Remarks { get; set; }

    /// <summary>总课序号</summary>
    [JsonPropertyName("kxh")]
    public string? TotalLessonNumber { get; set; }

    /// <summary>考试校区</summary>
    [JsonPropertyName("szxqmc")]
    public string? ExaminationCampus { get; set; }

    /// <summary>课程名称</summary>
    [JsonPropertyName("kcmc")]
    public string? CourseName { get; set; }

    /// <summary>平台编号</summary>
    [JsonPropertyName("kcptbh")]
    public string? CoursePlatformNumber { get; set; }

    /// <summary>教师姓名</summary>
    [JsonPropertyName("teaxms")]
    public string? Teacher { get; set; }

    /// <summary>上课日期</summary>
    [JsonPropertyName("skrq")]
    public DateTime? TeachingDate { get; set; }

    /// <summary>地址</summary><remarks>未使用</remarks>
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    /// <summary>任务类型</summary><remarks>var etype = {kb:'上课任务',ks:'考试任务',xk:'选课时间段'};</remarks>
    [JsonPropertyName("tasktype")]
    public int TaskTypeID { get; set; }

    /// <summary>教学班名称</summary>
    [JsonPropertyName("jxbmc")]
    public string? TeachingClass { get; set; }

    /// <summary>周次</summary>
    [JsonPropertyName("zc")]
    public int WeekNumber { get; set; }

    /// <summary>jxxsmc</summary>
    [JsonPropertyName("jxxsmc")]
    public string? Unknown_0 { get; set; }

    /// <summary>课程编号</summary>
    [JsonPropertyName("kcbh")]
    public string? CourseNumber { get; set; }

    /// <summary>教学场地</summary>
    [JsonPropertyName("jxcdmc")]
    public string? TeachingVenue { get; set; }

    /// <summary>背景色</summary>
    [JsonPropertyName("bgcolor")]
    public string? BgColor { get; set; }

    /// <summary>节次</summary>
    [JsonPropertyName("jcdm")]
    public string? OccupiedLessons { get; set; }

    /// <summary>教学楼</summary>
    [JsonPropertyName("jzwmc")]
    public string? TeachingBuilding { get; set; }

    /// <summary>项目名称</summary> // 吐槽：我不知道怎么翻译了
    [JsonPropertyName("xmmc")]
    public string? SubjectName { get; set; }

    /// <summary>节次2</summary>
    [JsonPropertyName("jcdm2")]
    public string? OccupiedLessons2 { get; set; }

    /// <summary>结束课序号</summary>
    [JsonPropertyName("pe")]
    public string? EndLessonNumber { get; set; }

    // ??? 某个编码
    [JsonPropertyName("dgksdm")]
    public string? Code { get; set; }

    /// <summary>校区名称</summary>
    [JsonPropertyName("xqmc")]
    public string? CampusName { get; set; }

    /// <summary>结束日期</summary>
    [JsonConverter(typeof(NullableDateTimeJsonConverter))]
    [JsonPropertyName("jsrq")]
    public DateTime? EndDate { get; set; }

    /// <summary>星期</summary>
    [JsonPropertyName("xq")]
    public int Weekday { get; set; }

    [JsonPropertyName("$ref")]
    public string? Ref { get; set; }

    [JsonIgnore]
    public DateTime StartDateTime => new(StartDate?.Year ?? 1, StartDate?.Month ?? 1, StartDate?.Day ?? 1, StartTime?.Hour ?? 0, StartTime?.Minute ?? 0, StartTime?.Second ?? 0);
    [JsonIgnore]
    public DateTime EndDateTime => new(EndDate?.Year ?? 1, EndDate?.Month ?? 1, EndDate?.Day ?? 1, EndTime?.Hour ?? 0, EndTime?.Minute ?? 0, EndTime?.Second ?? 0);

    [JsonIgnore]
    private static readonly string[] DayOfWeek = { "", "周一", "周二", "周三", "周四", "周五", "周六", "周日" };

    public override string ToString()
    {
        return new StringBuilder(CourseName).Append(Environment.NewLine)
            .Append(StartDateTime.ToString("yyyy-MM-dd HH:mm:ss")).Append(" ~ ").Append(EndDateTime.ToString("yyyy-MM-dd HH:mm:ss")).Append(Environment.NewLine)
            .Append('第').Append(WeekNumber).Append("周 ").Append(DayOfWeek[Weekday]).Append(' ').Append(OccupiedLessons2).Append('节').Append(Environment.NewLine)
            .Append(Teacher).Append(Environment.NewLine)
            .Append('[').Append(CampusName).Append(']').Append(TeachingVenue).Append('(').Append(TeachingBuilding).Append(')').Append(Environment.NewLine)
            .Append('[').Append(TeachingClass).Append(']').Append(SubjectName).Append(Environment.NewLine)
            .Append(Remarks).ToString();
    }

    public int CompareTo(Lesson? other)
    {
        if (other is null)
            return 1;
        int result;
        if ((result = WeekNumber.CompareTo(other.WeekNumber)) != 0)
            return result;
        if ((result = Weekday.CompareTo(other.Weekday)) != 0)
            return result;
        if (StartTime is null && other.StartTime is null)
            return 0;
        if (StartTime is null)
            return -1;
        if (other.StartTime is null)
            return 1;
        return StartTime.Value.CompareTo(other.StartTime.Value);
    }
}
