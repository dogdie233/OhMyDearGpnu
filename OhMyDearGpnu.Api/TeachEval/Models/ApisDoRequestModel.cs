namespace OhMyDearGpnu.Api.TeachEval.Models;

public class ApisDoRequestModel(SystemParamsModel systemParams, object? requestParams)
{
    public SystemParamsModel SystemParams { get; set; } = systemParams;
    public object? RequestParams { get; set; } = requestParams;
}