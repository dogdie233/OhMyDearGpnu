using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;

using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.Utility;

namespace OhMyDearGpnu.Api.TeachEval.Requests;

public class GetUserByContextTokenRequest(string token, string clientId) : ApisDoRequest<UserInfoModel>
{
    public override string ApiName => "Mycos.JP.Login.GetUserContextByToken";
    public override string RequestOriginPageAddress => $"{Hosts.teachEval}index.html?v=3.25.0#/user/init?token={HttpUtility.UrlEncode(token)}";

    public override object GetRequestParams()
    {
        return token;
    }

    public override HttpContent? CreateHttpContent(SimpleServiceContainer serviceContainer)
    {
        var systemParams = new SystemParamsModel
        {
            ApiName = ApiName,
            ClientId = clientId,
            ClientTime = DateTime.Now,
            RequestOriginPageAddress = RequestOriginPageAddress
        };

        var payload = new ApisDoRequestModel(systemParams, token);
        var payloadJson = JsonSerializer.Serialize(payload, TeachEvalSourceGeneratedJsonContext.Default.ApisDoRequestModel);
        var encryptedPayload = EncryptHelper.TeachEvalPayloadEncrypt(payloadJson + TeachEvalContext.PayloadTrailing);

        return new StringContent(encryptedPayload, new MediaTypeHeaderValue("application/json"));
    }
}