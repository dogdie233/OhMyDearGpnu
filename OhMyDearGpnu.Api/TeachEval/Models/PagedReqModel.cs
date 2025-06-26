using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public class PagedReqModel
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string? Direction { get; set; }
    [JsonPropertyName("IsGBKSort")] public bool IsGbkSort { get; set; } = false;

    public static PagedReqModel Default { get; set; } = new();
}