﻿using OhMyDearGpnu.Api.Common;

namespace OhMyDearGpnu.Api.StuAff.Requests;

[Request(PayloadTypeEnum.None)]
public partial class ServiceTicketLoginRequest(string ticket) : BaseRequest
{
    public override Uri Url { get; } = new(Hosts.stuAff, $"xgsy/shiro-cas?ticket={ticket}");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override ValueTask EnsureResponse(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        if (responseMessage.RequestMessage!.RequestUri!.Host == Hosts.cas.Host)
            throw new UnexpectedResponseException("Unable to login to Student affairs, maybe service ticket invalid");

        responseMessage.EnsureSuccessStatusCode();
        return ValueTask.CompletedTask;
    }
}