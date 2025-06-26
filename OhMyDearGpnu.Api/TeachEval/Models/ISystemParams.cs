namespace OhMyDearGpnu.Api.TeachEval.Models;

public interface ISystemParams
{
    string ApiName { get; }
    string BuildRequestOriginPageAddress(SystemParamsModel model);
    public PagedReqModel? PageContext => null;
    public string? Semester => null;
}