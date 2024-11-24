using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.IoT.Models;
using OhMyDearGpnu.Api.IoT.Requests;

namespace OhMyDearGpnu.Api.IoT;

public static class IoTContextExtension
{
    public static Task<DataResponse<List<RoomElectricModel>>> ListMyRoomElectric(this IoTContext context)
    {
        return context.GpnuClient.SendRequest(new ListMyRoomElectricRequest(context.Token));
    }

    public static Task<DataResponse<ElectricBalanceModel>> GetElectricBalance(this IoTContext context, string roomCode, string useEleType = "1")
    {
        return context.GpnuClient.SendRequest(new GetElectricBalanceRequest(context.Token, roomCode, useEleType));
    }
}