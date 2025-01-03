using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.StuAff.PartTimeJob.Models;
using OhMyDearGpnu.Api.StuAff.Requests;

namespace OhMyDearGpnu.Api.StuAff.PartTimeJob.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class QueryGridListSignsRequest(string token) : JsonApiRequestBase<GridListSignsModel>(token)
{
    public override Uri Url => new(Hosts.stuAff, "qgzx/api/sm-work-study/proData/queryAllConfig");
    public override HttpMethod HttpMethod => HttpMethod.Get;
}