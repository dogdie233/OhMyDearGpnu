using System.Web;

using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.IoT.Requests;

[Request(PayloadTypeEnum.None)]
public partial class ServiceTicketLoginRequest(string serviceTicket) : BaseRequest<string>
{
    public override Uri Url => new(Hosts.iot, $"kbp/auth/cas/auth/web/login?ticket={serviceTicket}");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override ValueTask<string> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        responseMessage.Content.Dispose();

        if (responseMessage.RequestMessage!.RequestUri!.Query is not { Length: >= 0 } query
            || HttpUtility.ParseQueryString(query).Get("token") is not { Length: >= 0 } token)
            throw new UnexpectedResponseException("Unable to get iot system token, maybe service ticket invalid");

        return ValueTask.FromResult(token);
    }
}