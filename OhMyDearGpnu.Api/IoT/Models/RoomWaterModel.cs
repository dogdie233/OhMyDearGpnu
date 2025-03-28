using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.IoT.Models;

public class RoomWaterModel
{
    [JsonPropertyName("roomCode")] public string RoomCode { get; set; } = string.Empty;
    [JsonPropertyName("areaInfo")] public string AreaInfo { get; set; } = string.Empty;
    [JsonPropertyName("userType")] public int UserType { get; set; }
    [JsonPropertyName("isAllowRecharge")] public bool IsAllowRecharge { get; set; }

    [JsonPropertyName("outstandingAmount")]
    public decimal OutstandingAmount { get; set; }

    [JsonPropertyName("payModel")] public int PayModel { get; set; }
}