using System.Web;

using OhMyDearGpnu.Api.TeachEval.Models;

namespace OhMyDearGpnu.Api.TeachEval;

public partial record StudentQuestionnairePageUrl
{
    public required string Type { get; init; } = "StudentReporter";

    public required string QuestionnaireId { get; init; }

    public required string TeacherCode { get; init; }

    public required string CourseCode { get; init; }

    public string? Semester { get; init; }

    public override string ToString()
    {
        return $"{Hosts.teachEval}index.html?v=3.38.0{ToFragment()}";
    }

    public string ToFragment()
    {
        var query = $"type={Type}&Qid={QuestionnaireId}&TeacherCode={TeacherCode}&CourseCode={CourseCode}";
        if (Semester is not null) query += $"&semester={Semester}";
        return $"#/answer/multipleAnswer?{query}";
    }

    public StudentQuestionnairePageUrl With(StudentTaskModel task)
    {
        return this with
        {
            QuestionnaireId = task.QuestionnaireId.ToString(),
            TeacherCode = task.TeacherCode,
            CourseCode = task.CourseCode,
            Type = task.TypeCode
        };
    }

    public static StudentQuestionnairePageUrl CreateFromFullUrl(Uri uri)
    {
        var uriStr = uri.ToString();
        var fragmentIndex = uriStr.IndexOf("#/answer/multipleAnswer?", StringComparison.OrdinalIgnoreCase);
        if (fragmentIndex == -1)
            throw new ArgumentException("Invalid URL format for StudentQuestionnairePageUrl.", nameof(uri));

        var queryString = uriStr[(fragmentIndex + 24)..];
        var query = HttpUtility.ParseQueryString(queryString);

        return new StudentQuestionnairePageUrl
        {
            Type = query.Get("type") ?? "StudentReporter",
            QuestionnaireId = query.Get("Qid") ?? "",
            TeacherCode = query.Get("TeacherCode") ?? "",
            CourseCode = query.Get("CourseCode") ?? "",
            Semester = query.Get("semester")
        };
    }

    public static StudentQuestionnairePageUrl CreateFromStudentTask(StudentTaskModel taskModel, string? semester = null)
    {
        return new StudentQuestionnairePageUrl
        {
            Type = taskModel.TypeCode,
            QuestionnaireId = taskModel.QuestionnaireId.ToString(),
            TeacherCode = taskModel.TeacherCode,
            CourseCode = taskModel.CourseCode,
            Semester = semester
        };
    }
}