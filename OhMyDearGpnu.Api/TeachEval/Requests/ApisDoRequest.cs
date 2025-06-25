using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public abstract class ApisDoRequest<T> : BaseRequest<T>, ISystemParams
{
    [JsonIgnore] public override Uri Url => new(Hosts.teachEval, "service/apis.do?ApiName=" + ApiName);
    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;

    [JsonIgnore] public abstract string ApiName { get; }
    [JsonIgnore] public abstract string RequestOriginPageAddress { get; }

    public virtual object GetRequestParams()
    {
        return this;
    }

    public override HttpContent? CreateHttpContent(SimpleServiceContainer serviceContainer)
    {
        var context = serviceContainer.Locate<TeachEvalContext>();
        var systemParams = context.CreateSystemParams(this);
        var requestParams = GetRequestParams();
        var payload = new ApisDoRequestModel(systemParams, requestParams);
        var payloadJson = JsonSerializer.Serialize(payload, TeachEvalSourceGeneratedJsonContext.Default.ApisDoRequestModel);
        var encryptedPayload = EncryptHelper.TeachEvalPayloadEncrypt(payloadJson + TeachEvalContext.PayloadTrailing);

        return new StringContent(encryptedPayload, new MediaTypeHeaderValue("application/json"));
    }

    public override async ValueTask<T> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        var responseDocument = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
        var root = responseDocument.RootElement;
        var valueJson = root.GetProperty("Value")!.ToString();
        return (T)JsonSerializer.Deserialize(valueJson, TeachEvalSourceGeneratedJsonContext.Default.GetTypeInfo(typeof(T))!)!;
    }
}

public abstract class ApisDoRequestPaged<T> : ApisDoRequest<PagedResponseModel<T>>
{
    public override async ValueTask<PagedResponseModel<T>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        var responseDocument = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
        var root = responseDocument.RootElement;
        var valueJson = root.GetProperty("Value")!.ToString();
        var pagedRaw = JsonSerializer.Deserialize(valueJson, TeachEvalSourceGeneratedJsonContext.Default.PagedResponseRawModel)!;
        return pagedRaw.Friendly<T>();
    }
}