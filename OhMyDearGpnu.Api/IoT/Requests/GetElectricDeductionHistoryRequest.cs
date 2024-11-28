using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.IoT.Models;

namespace OhMyDearGpnu.Api.IoT.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class GetElectricDeductionHistoryRequest(string token, int pageNumber, int pageSize, string roomCode, string username, int useEleType)
    : IoTApiRequestBase<ElectricDeductionHistoryModel>(token)
{
    [JsonIgnore] public override Uri Url => new(Hosts.iot, "kbp/ele/wechat/pay/deduction/list");
    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;

    [JsonPropertyName("pageNumber")] public int PageNumber { get; set; } = pageNumber;
    [JsonPropertyName("pageSize")] public int PageSize { get; set; } = pageSize;
    [JsonPropertyName("roomCode")] public string RoomCode { get; set; } = roomCode;
    [JsonPropertyName("useEleType")] public int UseEleType { get; set; } = useEleType;
    [JsonPropertyName("username")] public string Username { get; set; } = username;
}