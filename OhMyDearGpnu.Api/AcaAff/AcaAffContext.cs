using OhMyDearGpnu.Api.AcaAff.Requests;

namespace OhMyDearGpnu.Api.AcaAff;

public class AcaAffContext(GpnuClient client)
{
    public GpnuClient Client { get; init; } = client;

    public static async Task<AcaAffContext> CreateByServiceTicket(GpnuClient client)
    {
        client.cas.EnsureLoggedIn();
        var st = await client.cas.GenerateServiceTicket(Hosts.acaAff + "sso/lyiotlogin");
        await client.SendRequest(new ServiceTicketLoginRequest(st));
        return new AcaAffContext(client);
    }
}