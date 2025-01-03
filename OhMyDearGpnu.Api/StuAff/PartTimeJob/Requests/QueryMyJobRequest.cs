using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.StuAff.Models;
using OhMyDearGpnu.Api.StuAff.PartTimeJob.Models;
using OhMyDearGpnu.Api.StuAff.Requests;

namespace OhMyDearGpnu.Api.StuAff.PartTimeJob.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class QueryMyJobRequest(string token, PaginationModel pagination) : JsonApiRequestBase<Paged<MyJobModel>>(token)
{
    [JsonIgnore] public override Uri Url => new(Hosts.stuAff, "qgzx/api/sm-work-study/application/queryMyRzgw");
    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;

    [JsonPropertyName("pagination")] public PaginationModel Pagination { get; set; } = pagination;

    // [JsonPropertyName("filters")] public object Filters { get; set; } = new { };
    // [JsonPropertyName("sorter")] public object Sorter { get; set; } = new { };
    // [JsonPropertyName("searchTextMap")] public object SearchTextMap { get; set; } = new { };
    [JsonPropertyName("searchParams")] public Dictionary<string, List<string>> SearchParams { get; set; } = new();

    [JsonPropertyName("sqlParams")] public Dictionary<string, string> SqlParams { get; set; } = new();
    // [JsonPropertyName("initParams")] public object InitParams { get; set; } = new { };
}