using System.Text.Json;
using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.TeachEval.Requests;

namespace OhMyDearGpnu.Api.TeachEval;

[JsonSerializable(typeof(int?))]
[JsonSerializable(typeof(PagedReqModel))]
[JsonSerializable(typeof(SystemParamsModel))]
[JsonSerializable(typeof(ApisDoRequestModel))]
[JsonSerializable(typeof(PagedResponseRawModel))]
[JsonSerializable(typeof(UserInfoModel))]
[JsonSerializable(typeof(GetMyTaskItemByAnswerStatusRequest))]
[JsonSerializable(typeof(List<TaskItemModel>))]
[JsonSerializable(typeof(GetMyTaskItemDetailRequest))]
[JsonSerializable(typeof(List<TaskItemDetailModel>))]
[JsonSerializable(typeof(GetPublicKeyRequest))]
[JsonSerializable(typeof(LoginKeysModel))]
[JsonSerializable(typeof(GetFinalQuestionnaireHeaderRequest))]
[JsonSerializable(typeof(QuestionnaireHeaderModel))]
[JsonSerializable(typeof(GetQuestionnaireRequest))]
[JsonSerializable(typeof(QuestionnaireModel))]
[JsonSerializable(typeof(SaveQuestionnaireAnswerRequest))]
[JsonSourceGenerationOptions(JsonSerializerDefaults.General, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
internal partial class TeachEvalSourceGeneratedJsonContext : JsonSerializerContext
{
}