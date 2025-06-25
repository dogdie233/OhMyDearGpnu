using System.Text.Json;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public class PagedResponseModel<T>
{
    public int TotalCount { get; set; }
    public List<T> Items { get; set; } = [];
    public int PageIndex { get; set; }
    public bool IsOutRange { get; set; }
    public bool HasMaster { get; set; }
}

public class PagedResponseRawModel
{
    public int TotalCount { get; set; }
    public JsonDocument Items { get; set; } = null!;
    public int PageIndex { get; set; }
    public bool IsOutRange { get; set; }
    public bool HasMaster { get; set; }

    public PagedResponseModel<T> Friendly<T>()
    {
        var listTypeInfo = TeachEvalSourceGeneratedJsonContext.Default.GetTypeInfo(typeof(List<T>))!;
        var items = JsonSerializer.Deserialize(Items.RootElement.ToString(), listTypeInfo) as List<T> ?? [];
        return new PagedResponseModel<T>
        {
            TotalCount = TotalCount,
            Items = items,
            PageIndex = PageIndex,
            IsOutRange = IsOutRange,
            HasMaster = HasMaster
        };
    }
}