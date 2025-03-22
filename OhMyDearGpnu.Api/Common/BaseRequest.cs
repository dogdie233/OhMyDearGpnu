using System.Net.Http.Headers;
using System.Reflection;

namespace OhMyDearGpnu.Api.Common;

public abstract class BaseRequest
{
    public abstract Uri Url { get; }
    public abstract HttpMethod HttpMethod { get; }

    public virtual ValueTask FillAutoFieldAsync(SimpleServiceContainer serviceContainer)
    {
        return ValueTask.CompletedTask;
    }

    public virtual AuthenticationHeaderValue? GetAuthenticationHeaderValue(SimpleServiceContainer serviceContainer)
    {
        return null;
    }

    public abstract HttpContent? CreateHttpContent(SimpleServiceContainer serviceContainer);

    public abstract ValueTask EnsureResponse(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage);
}

public abstract class BaseRequest<TData> : BaseRequest
{
    public override ValueTask EnsureResponse(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        return ValueTask.CompletedTask;
    }

    public abstract ValueTask<TData> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage);
}