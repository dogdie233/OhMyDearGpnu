namespace OhMyDearGpnu.Api.TeachEval.Models;

public interface ISystemParams
{
    string ApiName { get; }
    string RequestOriginPageAddress { get; }
    public PagedModel? PageContext => null;
    public string? Semester => null;
}