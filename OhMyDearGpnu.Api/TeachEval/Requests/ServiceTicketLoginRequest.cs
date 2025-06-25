using System.Net;
using System.Web;

using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public class ServiceTicketLoginRequest(string ticket) : BaseRequest<string>
{
    public override Uri Url => new(Hosts.teachEval, $"DealSSO.ashx/?universitycode=10588_1&ticket={ticket}");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override HttpContent? CreateHttpContent(SimpleServiceContainer serviceContainer)
    {
        return null;
    }

    public override ValueTask<string> CreateDataResponseAsync(SimpleServiceContainer serviceContainer,
        HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        if (responseMessage.RequestMessage!.RequestUri!.Fragment is not { Length: >= 0 } fragment
            || !fragment.Contains('?')
            || HttpUtility.ParseQueryString(fragment[fragment.IndexOf('?')..]).Get("token") is not { Length: >= 0 } token)
            throw new UnexpectedResponseException("Unable to get teaching eval system token, maybe service ticket invalid");

        return ValueTask.FromResult(token);
    }
}