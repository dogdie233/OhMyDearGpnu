using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.IoT.Models;
using OhMyDearGpnu.Api.IoT.Requests;

namespace OhMyDearGpnu.Api.IoT;

public static class IoTContextExtension
{
    public static ValueTask<List<RoomElectricModel>> ListMyRoomElectric(this IoTContext context)
    {
        return context.GpnuClient.SendRequest(new ListMyRoomElectricRequest(context.Token));
    }

    public static ValueTask<ElectricBalanceModel> GetElectricBalance(this IoTContext context, string roomCode, string useEleType = "1")
    {
        return context.GpnuClient.SendRequest(new GetElectricBalanceRequest(context.Token, roomCode, useEleType));
    }

    public static ValueTask<ElectricDeductionHistoryModel> GetElectricDeductionHistory(this IoTContext context, int pageNumber, string roomCode, string username, int pageSize = 15, int useEleType = 1)
    {
        return context.GpnuClient.SendRequest(new GetElectricDeductionHistoryRequest(context.Token, pageNumber, pageSize, roomCode, username, useEleType));
    }
}