using System.Net;

using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.AcaAff.Requests;

[Request(PayloadTypeEnum.None)]
public partial class ServiceTicketLoginRequest(string ticket) : BaseRequest
{
    public override Uri Url => new(Hosts.acaAff, $"sso/lyiotlogin?ticket={ticket}");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override ValueTask EnsureResponse(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            throw new UnexpectedResponseException("Unable to login Academic affairs, maybe service ticket invalid");

        responseMessage.EnsureSuccessStatusCode();
        if (responseMessage.RequestMessage!.RequestUri!.Host != Hosts.acaAff.Host
            || responseMessage.RequestMessage!.RequestUri!.AbsolutePath != "/jwglxt/xtgl/index_initMenu.html")
            throw new UnexpectedResponseException("Login failed, unknown reason");

        return ValueTask.CompletedTask;
    }
}