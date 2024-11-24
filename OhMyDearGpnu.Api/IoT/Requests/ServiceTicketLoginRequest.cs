using System.Web;

using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.IoT.Requests;

[Request(PayloadTypeEnum.None)]
public partial class ServiceTicketLoginRequest(string serviceTicket) : BaseWithDataResponseRequest<string>
{
    public override Uri Url => new(Hosts.iot, $"kbp/auth/cas/auth/web/login?ticket={serviceTicket}");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override Task<DataResponse<string>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        responseMessage.Content.Dispose();

        if (responseMessage.RequestMessage!.RequestUri!.Query is not { Length: >= 0 } query
            || HttpUtility.ParseQueryString(query).Get("token") is not { Length: >= 0 } token)
            return Task.FromResult(DataResponse<string>.Fail("Unable to get iot system token, maybe service ticket invalid"));

        return Task.FromResult(DataResponse<string>.Success(token));
    }
}