using OhMyDearGpnu.Api.Responses;

namespace OhMyDearGpnu.Api.Requests
{
    public class GetTimetableRequest : BaseRequest
    {
        public override string Path => "/jwglxt/kbcx/xskbcx_cxXsgrkb.html";
        public override HttpMethod HttpMethod => HttpMethod.Post;

        public override Task<Response> CreateResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
        {
        }
    }
}
