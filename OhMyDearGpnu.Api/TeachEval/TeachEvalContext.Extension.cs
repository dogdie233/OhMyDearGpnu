using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.TeachEval.Requests;

namespace OhMyDearGpnu.Api.TeachEval;

public static class TeachEvalContextExtension
{
    public static Task<PagedResponseModel<TaskItemModel>> GetMyTaskItemByAnswerStatus(this TeachEvalContext context, string status = "UnFinished")
    {
        return context.GpnuClient.SendRequest(new GetMyTaskItemByAnswerStatusRequest(PagedModel.Default)
        {
            Status = status
        });
    }
}