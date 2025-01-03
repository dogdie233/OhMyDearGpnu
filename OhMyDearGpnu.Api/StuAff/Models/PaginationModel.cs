using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.StuAff.Models;

public class PaginationModel
{
    public PaginationModel()
    {
    }

    public PaginationModel(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    [JsonPropertyName("page")] public int Page { get; set; }
    [JsonPropertyName("pageSize")] public int PageSize { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
}