using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.Responses
{
    public class CurriculumInfo
    {
        [Alias("cdmc")] public string Classroom { get; set; }
        [Alias("rk")] public int Day { get; set; }
        public string WeekString { get; set; } = string.Empty;
        [Alias("zcd")] public DiscreteNumberRange Week { get; set; }
        [Alias("kcmc")] public string Name { get; set; }
        public string TimeIdString { get; set; } = string.Empty;
        [Alias("jcs")] public DiscreteNumberRange TimeId { get; set; }
        [Alias("xqmc")] public string Campus { get; set; }
        [Alias("xm")] public string TeacherName { get; set; }
        [Alias("khfsmc")] public string AssessmentTypeName { get; set; }
        [Alias("xkbz")] public string Comment { get; set; }
        [Alias("kcxszc")] public string CompositionOfCourseHours { get; set; }
        [Alias("zhxs")] public int WeeklyCourseHours { get; set; }
        [Alias("zxs")] public int TotalCourseHours { get; set; }
        [Alias("xf")] public float Credit { get; set; }
    }
}
