using System.Net.Http.Headers;
using System.Net.Http.Json;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.IoT.Models;

namespace OhMyDearGpnu.Api.IoT.Requests;

public abstract class IoTApiRequestBase<T> : BaseWithDataResponseRequest<T> where T : class
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

    public override async Task<DataResponse<T>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();
        var res = await responseMessage.Content.ReadFromJsonAsync<IoTApiResponseModelBase<T>>();
        var data = res?.Data;
        return data == null ? DataResponse<T>.Fail("Return a empty response") : DataResponse<T>.Success(data);
    }
}