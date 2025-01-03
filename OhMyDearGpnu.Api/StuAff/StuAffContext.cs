using OhMyDearGpnu.Api.StuAff.Models;
using OhMyDearGpnu.Api.StuAff.Requests;

namespace OhMyDearGpnu.Api.StuAff;

public class StuAffContext(GpnuClient gpnuClient, UserInfoModel userInfo)
{
    public GpnuClient GpnuClient { get; init; } = gpnuClient;
    public UserInfoModel UserInfo { get; init; } = userInfo;
    public string Token => UserInfo.TokenId;

    public static async Task<StuAffContext> CreateByServiceTicket(GpnuClient client)
    {
        client.cas.EnsureLoggedIn();
        var st = await client.cas.GenerateServiceTicket(Hosts.stuAff + "xgsy/shiro-cas&loginToken=loginToken");
        await client.SendRequest(new ServiceTicketLoginRequest(st));
        var userInfo = await client.SendRequest(new GetUserInfoRequest());
        return new StuAffContext(client, userInfo);
    }
}