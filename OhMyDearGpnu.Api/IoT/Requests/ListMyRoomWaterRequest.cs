using System.Text.Json.Serialization;
using System.Web;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.IoT.Models;

namespace OhMyDearGpnu.Api.IoT.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class ListMyRoomWaterRequest(string token, string username, int type = 1) : IoTApiRequestBase<List<RoomWaterModel>>(token)
{
    [JsonIgnore] public override Uri Url => new(Hosts.iot, $"kbp/cwbs/rest/coldWater/room/list?username={HttpUtility.UrlEncode(username)}&type={type}");
    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Get;
}