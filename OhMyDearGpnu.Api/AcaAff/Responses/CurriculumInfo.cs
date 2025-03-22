using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.AcaAff.Responses;

public class CurriculumInfo
{
    [Alias("cdmc")] public string Classroom { get; set; } = string.Empty;
    [Alias("rk")] public int Day { get; set; }
    public string WeekString { get; set; } = string.Empty;
    [Alias("zcd")] public DiscreteNumberRange Week { get; set; }
    [Alias("kcmc")] public string Name { get; set; } = string.Empty;
    public string TimeIdString { get; set; } = string.Empty;
    [Alias("jcs")] public DiscreteNumberRange TimeId { get; set; }
    [Alias("xqmc")] public string Campus { get; set; } = string.Empty;
    [Alias("xm")] public string TeacherName { get; set; } = string.Empty;
    [Alias("khfsmc")] public string AssessmentTypeName { get; set; } = string.Empty;
    [Alias("xkbz")] public string Comment { get; set; } = string.Empty;
    [Alias("kcxszc")] public string CompositionOfCourseHours { get; set; } = string.Empty;
    [Alias("zhxs")] public int WeeklyCourseHours { get; set; }
    [Alias("zxs")] public int TotalCourseHours { get; set; }
    [Alias("xf")] public float Credit { get; set; }
}