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
        var token = await client.SendRequest(new ServiceTicketLoginRequest(st));
        return new IoTContext(client, token);
    }
}