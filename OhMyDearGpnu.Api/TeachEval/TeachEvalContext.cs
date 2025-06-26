using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.TeachEval.Requests;

namespace OhMyDearGpnu.Api.TeachEval;

public class TeachEvalContext(GpnuClient gpnuClient, UserInfoModel userInfo, string clientId)
{
    public static readonly string PayloadTrailing = "d^PrEK&c";
    public GpnuClient GpnuClient { get; } = gpnuClient;
    public UserInfoModel UserInfo { get; } = userInfo;

    public string Token => UserInfo.Token;

    public SystemParamsModel SharedSystemParams { get; } = new()
    {
        ApiName = string.Empty,
        ClientId = clientId,
        RequestOriginPageAddress = string.Empty
    };

    public static async Task<TeachEvalContext> CreateByServiceTicket(GpnuClient client)
    {
        client.cas.EnsureLoggedIn();
        var st = await client.cas.GenerateServiceTicket(Hosts.teachEval + "DealSSO.ashx/?universitycode=10588_1&loginToken=loginToken");
        var token = await client.SendRequest(new ServiceTicketLoginRequest(st));
        var clientId = Guid.NewGuid().ToString("N");
        var userInfo = await client.SendRequest(new GetUserByContextTokenRequest(token, clientId));
        return new TeachEvalContext(client, userInfo, clientId);
    }

    public SystemParamsModel CreateSystemParams(ISystemParams mergeParams)
    {
        var p = SharedSystemParams with
        {
            ApiName = mergeParams.ApiName,
            RequestOriginPageAddress = null!,
            PageContext = mergeParams.PageContext,
            Semester = mergeParams.Semester ?? UserInfo.CurrentSemester,
            ClientTime = DateTime.Now,
            Token = Token,
            UserCode = UserInfo.Code,
            UniversityCode = UserInfo.UniversityCode
        };
        p.RequestOriginPageAddress = mergeParams.BuildRequestOriginPageAddress(p);
        return p;
    }
}