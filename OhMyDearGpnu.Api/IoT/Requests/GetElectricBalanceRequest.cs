using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.IoT.Models;

namespace OhMyDearGpnu.Api.IoT.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class GetElectricBalanceRequest(string token, string roomCode, string useEleType = "1") : IoTApiRequestBase<ElectricBalanceModel>(token)
{
    [JsonIgnore] public override Uri Url => new(Hosts.iot, "kbp/ele/wechat/ele/eleBalance");
    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;
    [JsonPropertyName("roomCode")] public string RoomCode { get; set; } = roomCode;
    [JsonPropertyName("useEleType")] public string UseEleType { get; set; } = useEleType;
}