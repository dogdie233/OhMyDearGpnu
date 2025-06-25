using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;
using OhMyDearGpnu.Api.TeachEval.Models;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public class GetMyTaskItemByAnswerStatusRequest(PagedModel paged) : ApisDoRequestPaged<TaskItemModel>, ISystemParams
{
    public string Source { get; set; } = "pc";
    public string Status { get; set; } = "UnFinished";
    public int? EvaType { get; set; } = null;
    public string? EvaCode { get; set; } = null;

    [JsonConverter(typeof(BooleanNumberConverter))]
    public bool IsIncludeHistorySemester { get; set; } = true;

    [JsonIgnore] public PagedModel PagedContext { get; set; } = paged;
    [JsonIgnore] public string? Semester { get; set; }

    [JsonIgnore] PagedModel ISystemParams.PageContext => PagedContext;
    [JsonIgnore] string? ISystemParams.Semester => Semester;

    [JsonIgnore] public override string ApiName => "Mycos.JP.MyTask.MyTask.GetMyTaskItemByAnswerStatus";

    [JsonIgnore] public override string RequestOriginPageAddress => $"{Hosts.teachEval}index.html?v=3.25.0#/my-task/main/UnFinished";
}