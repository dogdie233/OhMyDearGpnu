using System.Net.Http.Json;

using OhMyDearGpnu.Api.StuAff.Requests;

namespace OhMyDearGpnu.Api.StuAff.PartTimeJob.Requests;

public class DeleteWorkloadItemRequest(IEnumerable<string> registrationIds) : JsonApiRequestBase<int?>
{
    public override Uri Url => new(Hosts.stuAff, "qgzx/api/sm-work-study/jobRegister/deleteBatch");
    public override HttpMethod HttpMethod => HttpMethod.Post;

    public override HttpContent? CreateHttpContent(SimpleServiceContainer serviceContainer)
    {
        return JsonContent.Create(registrationIds, StuAffSourceGeneratedJsonContext.Default.GetTypeInfo(typeof(IEnumerable<string>))!);
    }
}