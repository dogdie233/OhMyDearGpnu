using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public abstract class ApisDoRequest<T> : TeachEvalRequest<T>, ISystemParams
{
    [JsonIgnore] public override Uri Url => new(Hosts.teachEval, "service/apis.do?ApiName=" + ApiName);

    [JsonIgnore] public override HttpMethod HttpMethod => HttpMethod.Post;
}

public abstract class ApisDoRequestPaged<T> : ApisDoRequest<PagedResponseModel<T>>, ISystemParams
{
    [JsonIgnore] public PagedReqModel Paged { get; set; } = PagedReqModel.Default;

    [JsonIgnore] PagedReqModel ISystemParams.PageContext => Paged;

    public override async ValueTask<PagedResponseModel<T>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        var responseDocument = await JsonDocument.ParseAsync(await responseMessage.Content.ReadAsStreamAsync());
        var root = responseDocument.RootElement;
        var valueJson = root.GetProperty("Value")!.ToString();
        var pagedRaw = JsonSerializer.Deserialize(valueJson, TeachEvalSourceGeneratedJsonContext.Default.PagedResponseRawModel)!;
        return pagedRaw.Friendly<T>();
    }
}