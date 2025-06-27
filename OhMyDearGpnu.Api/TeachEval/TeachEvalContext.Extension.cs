using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.TeachEval.Requests;

namespace OhMyDearGpnu.Api.TeachEval;

public static class TeachEvalContextExtension
{
    public static Task<PagedResponseModel<TaskItemModel>> GetMyTaskItemByAnswerStatus(this TeachEvalContext context)
    {
        return context.GpnuClient.SendRequest(new GetMyTaskItemByAnswerStatusRequest());
    }
}