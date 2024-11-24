using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.IoT.Models;

public class ElectricBalanceModel
{
    [JsonPropertyName("roomCode")] public string RoomCode { get; set; } = string.Empty;
    [JsonPropertyName("areaInfo")] public string AreaInfo { get; set; } = string.Empty;
    [JsonPropertyName("moneyBalance")] public double MoneyBalance { get; set; }

    [JsonPropertyName("itemSubsidyBalance")]
    public double ItemSubsidyBalance { get; set; }

    [JsonPropertyName("moneySubsidyBalance")]
    public double MoneySubsidyBalance { get; set; }

    [JsonPropertyName("todayBalance")] public double TodayBalance { get; set; }
    [JsonPropertyName("monthBalance")] public double MonthBalance { get; set; }
}