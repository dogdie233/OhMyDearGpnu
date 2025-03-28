using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.IoT.Models;

namespace OhMyDearGpnu.Api.IoT.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class GetWaterBalanceRequest(string token, string username, string roomCode) : IoTApiRequestBase<WaterBalanceModel>(token)
{
    [JsonIgnore] public override Uri Url => new(Hosts.iot, "kbp/cwbs/rest/coldWater/getDetails");
    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;
    [JsonPropertyName("username")] public string Username { get; set; } = username;
    [JsonPropertyName("roomCode")] public string RoomCode { get; set; } = roomCode;
}