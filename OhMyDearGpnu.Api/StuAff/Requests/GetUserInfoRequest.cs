using System.Net.Http.Json;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.StuAff.Models;
using OhMyDearGpnu.Api.StuAff.Responses;

namespace OhMyDearGpnu.Api.StuAff.Requests;

[Request(PayloadTypeEnum.None)]
public partial class GetUserInfoRequest : BaseRequest<UserInfoModel>
{
    public override Uri Url => new(Hosts.stuAff, "xgsy/tryLoginUserInfo");
    public override HttpMethod HttpMethod => HttpMethod.Post;

    public override async ValueTask<UserInfoModel> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        responseMessage.EnsureSuccessStatusCode();

        var res = await responseMessage.Content.ReadFromJsonAsync(StuAffSourceGeneratedJsonContext.Default.GetUserInfoResponse);
        if (res is not { Meta: not null, Data: not null })
            throw new UnexpectedResponseException("Unable to get user info, response json invalid");

        if (res.Meta.StatusCode != 200)
            throw new UnexpectedResponseException("Unable to get user info, " + res.Meta.Message);

        return res.Data;
    }
}