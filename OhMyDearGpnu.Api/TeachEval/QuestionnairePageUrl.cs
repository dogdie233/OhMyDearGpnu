using System.Text.RegularExpressions;
using System.Web;

using OhMyDearGpnu.Api.TeachEval.Models;

namespace OhMyDearGpnu.Api.TeachEval;

public partial record QuestionnairePageUrl
{
    public string QuestionnaireId { get; init; } = "undefined";
    public required string QuestionnaireTypeCode { get; init; } = "1";
    public required string QuestionnaireType { get; init; } = "Final";
    public required string TaskId { get; init; } = "1";
    public required string CurrentCourseCode { get; init; } = "114514";
    public required string Status { get; init; } = "UnFinished";
    public required string TaskStatus { get; init; } = "1";
    public string? Semester { get; init; }

    public override string ToString()
    {
        return $"{Hosts.teachEval}index.html?v=3.25.0${ToFragment()}";
    }

    public string ToFragment()
    {
        var query = Semester is not null ? $"?semester={Semester}" : "";
        return
            $"#/my-task/answer/zkd/{Status}/{TaskStatus}/{QuestionnaireTypeCode}/{QuestionnaireType}/{QuestionnaireId}/{CurrentCourseCode}/{TaskId}{query}";
    }

    public QuestionnairePageUrl With(TaskItemDetailModel taskItemDetail)
    {
        return this with
        {
            QuestionnaireId = taskItemDetail.QuestionnaireId,
            CurrentCourseCode = taskItemDetail.CourseCode,
            TaskStatus = taskItemDetail.Status.ToString()
        };
    }

    public static QuestionnairePageUrl CreateFromFullUrl(Uri uri)
    {
        var uriStr = uri.ToString();
        var match = RegexHelper.QuestionnairePageFragmentRegex().Match(uriStr);
        if (!match.Success)
            throw new ArgumentException("Invalid URL format for QuestionnairePageContext.", nameof(uri));

        string? semester = null;
        if (match.Index + match.Length < uriStr.Length)
        {
            var query = HttpUtility.ParseQueryString(uriStr[(match.Index + match.Length)..]);
            semester = query.Get("semester");
        }

        return new QuestionnairePageUrl
        {
            Status = match.Groups[1].Value,
            TaskStatus = match.Groups[2].Value,
            QuestionnaireTypeCode = match.Groups[3].Value,
            QuestionnaireType = match.Groups[4].Value,
            QuestionnaireId = match.Groups[5].Value,
            CurrentCourseCode = match.Groups[6].Value,
            TaskId = match.Groups[7].Value,
            Semester = semester
        };
    }

    public static QuestionnairePageUrl CreateFromTaskItem(TaskItemModel taskItem)
    {
        return new QuestionnairePageUrl
        {
            Status = "UnFinished",
            TaskStatus = taskItem.TaskStatus.ToString(),
            QuestionnaireTypeCode = taskItem.EvaType.ToString(),
            QuestionnaireType = taskItem.EvaCode,
            QuestionnaireId = "undefined",
            CurrentCourseCode = "undefined",
            TaskId = taskItem.TaskId,
            Semester = taskItem.Semester
        };
    }

    internal static partial class RegexHelper
    {
        // "#/my-task/answer/zkd/UnFinished/1/2/Final/7/114514/8?semester=2024-2025-2"
        [GeneratedRegex(@"#/my-task/answer/zkd/(\w+)/(\d+)/(\d+)/(\w+)/(\w+)/(\d+)/(\d+)")]
        public static partial Regex QuestionnairePageFragmentRegex();
    }
}