using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.StuAff.PartTimeJob.Models;

public class GridListSignsModel
{
    [JsonPropertyName("table")] public Dictionary<string, TableItem> Table { get; set; } = new();

    public class TableItem
    {
        [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
        [JsonPropertyName("table")] public string? Table { get; set; }
        [JsonPropertyName("rowKey")] public string? RowKey { get; set; }
    }
}