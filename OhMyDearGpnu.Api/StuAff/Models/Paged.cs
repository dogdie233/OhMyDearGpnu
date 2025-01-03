using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.StuAff.Models;

public class Paged<T>
{
    [JsonPropertyName("list")] public List<T> List { get; set; } = [];
    [JsonPropertyName("pageCount")] public int PageCount { get; set; }
    [JsonPropertyName("pageNum")] public int PageNum { get; set; }
    [JsonPropertyName("pageSize")] public int PageSize { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
}