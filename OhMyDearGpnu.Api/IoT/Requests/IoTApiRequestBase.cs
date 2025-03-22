using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.IoT.Models;

namespace OhMyDearGpnu.Api.IoT.Requests;

public abstract class IoTApiRequestBase<T> : BaseRequest<T> where T : class
{
    private string token;

    protected IoTApiRequestBase(string token)
    {
        this.token = token;
    }

    public override AuthenticationHeaderValue? GetAuthenticationHeaderValue(SimpleServiceContainer serviceContainer)
    {
        return new AuthenticationHeaderValue(token);
    }

    public override async ValueTask<T> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        var res = await responseMessage.Content.ReadFromJsonAsync((JsonTypeInfo<IoTApiResponseModelBase<T>>)IoTSourceGeneratedJsonContext.Default.GetTypeInfo(typeof(IoTApiResponseModelBase<T>))!);
        if (res?.Data == null)
            throw new UnexpectedResponseException("Failed to deserialize the response data.");
        return res.Data;
    }
}