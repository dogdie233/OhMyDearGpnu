using OhMyDearGpnu.Api.IoT.Requests;

namespace OhMyDearGpnu.Api.IoT;

public class IoTContext(GpnuClient gpnuClient, string token)
{
    public GpnuClient GpnuClient { get; init; } = gpnuClient;
    public string Token { get; init; } = token;

    public static async Task<IoTContext> CreateByServiceTicket(GpnuClient client)
    {
        client.cas.EnsureLoggedIn();
        var st = await client.cas.GenerateServiceTicket(Hosts.iot + "kbp/auth/cas/auth/web/login");
        var tokenRes = await client.SendRequest(new ServiceTicketLoginRequest(st));
        if (!tokenRes.IsSucceeded)
            throw new Exception(tokenRes.message);

        return new IoTContext(client, tokenRes.data);
    }
}