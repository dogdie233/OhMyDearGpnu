using System.Net.Http.Headers;
using System.Net.Http.Json;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.StuAff.Models;

namespace OhMyDearGpnu.Api.StuAff.Requests;

public abstract class JsonApiRequestBase<T>(string token) : BaseRequest<T>
{
    public override AuthenticationHeaderValue? GetAuthenticationHeaderValue(SimpleServiceContainer serviceContainer)
    {
        return new AuthenticationHeaderValue(token);
    }

    public override async ValueTask<T> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        var res = await responseMessage.Content.ReadFromJsonAsync<JsonResponseModel<T>>();
        if (res == null)
            throw new UnexpectedResponseException($"Failed to deserialize the response data. Http Code: {responseMessage.StatusCode}");
        if (res.Data == null)
            throw new UnexpectedResponseException($"The response data is null. Code: {res.Code}, Message: {res.Message}");
        return res.Data;
    }
}