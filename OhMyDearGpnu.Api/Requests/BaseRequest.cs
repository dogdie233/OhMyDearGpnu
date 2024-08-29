using OhMyDearGpnu.Api.Responses;
using OhMyDearGpnu.Api.Utility;

using System.Reflection;

namespace OhMyDearGpnu.Api.Requests
{
    public abstract class BaseWithDataResponseRequest
    {
        public virtual string Host => "https://jwglxt.gpnu.edu.cn/";
        public abstract string Path { get; }
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

        public IEnumerable<KeyValuePair<string, string>> GetFormItems(SimpleServiceContainer serviceContainer)
        {
            var infos = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                .Select(field => (field: field, attributes: field.GetCustomAttributes().ToArray()))
                .Where(info => info.attributes.Any(a => a.GetType() == typeof(FormItemAttribute)))
                .ToArray();

            var result = new List<KeyValuePair<string, string>>(infos.Length);
            foreach (var info in infos)
            {
                var valueObj = info.field.GetValue(this);
                if (valueObj == null)
                    continue;

                var value = valueObj.ToString();
                var itemName = ((FormItemAttribute)info.attributes.First(attr => attr.GetType() == typeof(FormItemAttribute))).name;

                var item = RequestSerializationHelper.SeralizeField(info.field, this, info.attributes, itemName);
                if (item != null)
                    result.Add(item.Value);
            }
            PostProcessFormData(result);
            return result;
        }

        public abstract Task<Response> CreateResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage);

        protected virtual void PostProcessFormData(List<KeyValuePair<string, string>> data)
        {
        }
    }

    public abstract class BaseWithDataResponseRequest<TData> : BaseWithDataResponseRequest
    {
        public override async Task<Response> CreateResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
            => await CreateDataResponseAsync(serviceContainer, responseMessage);

        public abstract Task<DataResponse<TData>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage);
    }
}
