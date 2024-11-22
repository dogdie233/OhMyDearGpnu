using System.Net.Http.Json;

using OhMyDearGpnu.Api.Responses;
using OhMyDearGpnu.Api.Utility;

using System.Reflection;
using System.Text.Json;

namespace OhMyDearGpnu.Api.Requests;

public abstract class BaseWithDataResponseRequest
{
    public abstract Uri Url { get; }
    public abstract HttpMethod HttpMethod { get; }

    public virtual async Task FillAutoFieldAsync(SimpleServiceContainer serviceContainer)
    {
        var pageCacheManager = serviceContainer.Locate<PageCacheManager>();
        (FieldInfo field, FromPageCacheAttribute attribute)[] fromCachePageFields = GetType().GetFields()
            .Select(field => (field, field.GetCustomAttribute<FromPageCacheAttribute>()))
            .Where(info => info.Item2 != null)
            .ToArray()!;

        foreach (var info in fromCachePageFields)
        {
            var identifier = info.attribute.pageIdentifier;
            var cache = pageCacheManager.GetCache(identifier);
            if (cache == null)
                continue;

            if (cache.IsExpire)
                await cache.UpdateAsync();

            var value = info.attribute.ParseValue(cache);
            info.field.SetValue(this, value);
        }
    }

    public abstract HttpContent? CreateHttpContent(SimpleServiceContainer serviceContainer);

    public abstract Task<Response> CreateResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage);
}

public abstract class BaseWithDataResponseRequest<TData> : BaseWithDataResponseRequest
{
    public override async Task<Response> CreateResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        return await CreateDataResponseAsync(serviceContainer, responseMessage);
    }

    public abstract Task<DataResponse<TData>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage);
}