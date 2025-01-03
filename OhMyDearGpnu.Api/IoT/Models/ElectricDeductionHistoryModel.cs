using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

// ReSharper disable ClassNeverInstantiated.Global

namespace OhMyDearGpnu.Api.IoT.Models;

public class ElectricDeductionHistoryModel
{
    [JsonPropertyName("countId")] public int? CountId { get; set; }

    /// <summary>
    /// 等于请求的 pageNumber
    /// </summary>
    [JsonPropertyName("current")]
    public int Current { get; set; }

    [JsonPropertyName("hitCount")] public bool HitCount { get; set; }

    [JsonPropertyName("maxLimit")] public double? MaxLimit { get; set; }

    // [JsonPropertyName("optimizeCountSql")] public bool OptimizeCountSql { get; set; }
    // [JsonPropertyName("orders")] public List<dynamic> Orders { get; set; } = new();
    [JsonPropertyName("pages")] public int Pages { get; set; }
    [JsonPropertyName("records")] public List<RecordModel> Records { get; set; } = [];
    [JsonPropertyName("searchCount")] public bool SearchCount { get; set; }
    [JsonPropertyName("size")] public int Size { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }

    public class RecordModel
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("consumeSum")] public double ConsumeSum { get; set; }
        [JsonPropertyName("consumeAmount")] public double ConsumeAmount { get; set; }

        [JsonPropertyName("consumeTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ConsumeTime { get; set; }

        [JsonPropertyName("consumeTypeName")] public string? ConsumeTypeName { get; set; }

        [JsonPropertyName("consumeChannelName")]
        public string? ConsumeChannelName { get; set; }

        [JsonPropertyName("statusName")] public string? StatusName { get; set; }
        [JsonPropertyName("roomName")] public string? RoomName { get; set; }

        /// <summary>
        /// 如果这条数据是记录的第一条数据，则此值为null
        /// </summary>
        [JsonPropertyName("lastReportTime")]
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? LastReportTime { get; set; }

        [JsonPropertyName("reportTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ReportTime { get; set; }

        [JsonPropertyName("lastReportElectric")]
        public double LastReportElectric { get; set; }

        [JsonPropertyName("reportElectric")] public double ReportElectric { get; set; }

        /// <summary>
        /// 单位为分
        /// </summary>
        [JsonPropertyName("beforeBalance")]
        public long BeforeBalance { get; set; }

        /// <summary>
        /// 单位为分
        /// </summary>
        [JsonPropertyName("afterBalance")]
        public long AfterBalance { get; set; }
    }
}