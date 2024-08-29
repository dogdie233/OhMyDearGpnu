using OhMyDearGpnu.Api.Responses;

using System.Globalization;

using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.Requests
{
    public class PersonInfoRequest : BaseWithDataResponseRequest<PersonInfoResponse>
    {
        public override string Path => "/jwglxt/xsxxxggl/xsgrxxwh_cxXsgrxx.html?gnmkdm=N100801&layout=default";
        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override async Task<DataResponse<PersonInfoResponse>> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
        {
            var cacheManager = serviceContainer.Locate<PageCacheManager>();
            var cache = cacheManager.GetCache(Path);
            if (cache == null)
            {
                var reqMsg = new HttpRequestMessage(HttpMethod, Path);
                cache = PageCache.CreateLazy(serviceContainer.Locate<GpnuClient>(), reqMsg, TimeSpan.FromMinutes(10));
                cacheManager.AddCache(cache);
            }
            if (cache.IsExpire)
                await cache.UpdateAsync();

            var tab = cache.Document!.QuerySelector("#content_xsxxgl_xsjbxx");
            var groups = tab!.QuerySelectorAll(".form-group");

            var result = new PersonInfoResponse();
            var deserializer = Deserializer<PersonInfoResponse>.Create(result);
            foreach (var group in groups)
            {
                var labelText = group.QuerySelector("label")?.TextContent.Trim()[0..^1];  // 切掉最后一个“：”
                var valueText = group.QuerySelector("div")?.TextContent.Trim();

                if (labelText == null || valueText == null)
                    continue;

                object? value = labelText switch
                {
                    "出生日期" => !string.IsNullOrWhiteSpace(valueText) ? DateTime.Parse(valueText) : null,
                    "入学日期" => !string.IsNullOrWhiteSpace(valueText) ? DateTime.ParseExact(valueText, "yyyyMMdd", CultureInfo.InvariantCulture) : null,
                    _ => valueText
                };
                deserializer.Write(labelText, value);
            }

            return DataResponse<PersonInfoResponse>.Success(result);
        }
    }
}
