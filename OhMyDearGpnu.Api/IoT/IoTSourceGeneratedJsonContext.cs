using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.IoT.Models;
using OhMyDearGpnu.Api.IoT.Requests;

namespace OhMyDearGpnu.Api.IoT;

[JsonSerializable(typeof(ElectricBalanceModel))]
[JsonSerializable(typeof(ElectricDeductionHistoryModel))]
[JsonSerializable(typeof(RoomElectricModel))]
[JsonSerializable(typeof(GetElectricBalanceRequest))]
[JsonSerializable(typeof(IoTApiResponseModelBase<ElectricBalanceModel>))]
[JsonSerializable(typeof(GetElectricDeductionHistoryRequest))]
[JsonSerializable(typeof(IoTApiResponseModelBase<ElectricDeductionHistoryModel>))]
[JsonSerializable(typeof(ListMyRoomElectricRequest))]
[JsonSerializable(typeof(IoTApiResponseModelBase<List<RoomElectricModel>>))]
[JsonSerializable(typeof(ServiceTicketLoginRequest))]
public partial class IoTSourceGeneratedJsonContext : JsonSerializerContext { }