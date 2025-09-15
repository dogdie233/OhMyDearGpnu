using OhMyDearGpnu.Api.IoT.Requests;

namespace OhMyDearGpnu.Api.IoT;

public class IoTContext(GpnuClient gpnuClient, string token)
{
    private TaskCompletionSource? _tokenRefreshTcs;
    public GpnuClient GpnuClient { get; init; } = gpnuClient;
    public string Token { get; private set; } = token;
        

    public Task RefreshToken()
    {
        var newTcs = new TaskCompletionSource();
        var tcs = Interlocked.CompareExchange(ref _tokenRefreshTcs, newTcs, null);
        if (tcs != null)
            return tcs.Task;

        DoTokenRefresh();
        return newTcs.Task;
    }

    private async void DoTokenRefresh()
    {
        var tcs = _tokenRefreshTcs;
        try
        {
            var newCtx = await CreateByServiceTicket(GpnuClient);
            Token = newCtx.Token;
            _tokenRefreshTcs = null;
            tcs?.SetResult();
        }
        catch (Exception ex)
        {
            _tokenRefreshTcs = null;
            tcs?.SetException(ex);
        }
    }
    
    public async Task<TData> SendRequest<TData>(IoTApiRequestBase<TData> request) where TData : class
    {
        try
        {
            return await GpnuClient.SendRequest(request);
        }
        catch (TokenExpiredException ex) when (ex.Scope == typeof(IoTContext))
        {
            await RefreshToken();
            request.Token = Token;
        }

        return await GpnuClient.SendRequest(request);
    }

    public static async Task<IoTContext> CreateByServiceTicket(GpnuClient client)
    {
        client.cas.EnsureLoggedIn();
        var st = await client.cas.GenerateServiceTicket(Hosts.iot + "kbp/auth/cas/auth/web/login");
        var token = await client.SendRequest(new ServiceTicketLoginRequest(st));
        return new IoTContext(client, token);
    }
}