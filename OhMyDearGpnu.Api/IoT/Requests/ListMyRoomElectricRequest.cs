using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.IoT.Models;

namespace OhMyDearGpnu.Api.IoT.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class ListMyRoomElectricRequest(string token) : IoTApiRequestBase<List<RoomElectricModel>>(token)
{
    [JsonIgnore] public override Uri Url => new(Hosts.iot, "kbp/ele/mobile/assigned/room/list");
    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Get;
}