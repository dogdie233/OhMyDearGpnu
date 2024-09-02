using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.Responses
{
    public class CurriculumInfo
    {
        [Alias("cdmc")] public string Classroom { get; set; }
        [Alias("rk")] public int Day { get; set; }
        [Alias("zcd")] public DiscreteNumberRange Week { get; set; }
        [Alias("kcmc")] public string Name { get; set; }
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
