using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.IoT.Models;

public class RoomElectricModel
{
    [JsonPropertyName("userIdentity")] public int UserIdentity { get; set; }
    [JsonPropertyName("roomCode")] public string RoomCode { get; set; } = string.Empty;
    [JsonPropertyName("AreaInfo")] public string AreaInfo { get; set; } = string.Empty;
    [JsonPropertyName("useEleTypeList")] public List<int> UseElectricTypeList { get; set; } = [];
}