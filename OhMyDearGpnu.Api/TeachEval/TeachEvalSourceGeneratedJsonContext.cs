using System.Text.Json;
using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.TeachEval.Requests;

namespace OhMyDearGpnu.Api.TeachEval;

[JsonSerializable(typeof(int?))]
[JsonSerializable(typeof(PagedModel))]
[JsonSerializable(typeof(SystemParamsModel))]
[JsonSerializable(typeof(ApisDoRequestModel))]
[JsonSerializable(typeof(PagedResponseRawModel))]
[JsonSerializable(typeof(UserInfoModel))]
[JsonSerializable(typeof(GetMyTaskItemByAnswerStatusRequest))]
[JsonSerializable(typeof(List<TaskItemModel>))]
[JsonSourceGenerationOptions(JsonSerializerDefaults.General, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
internal partial class TeachEvalSourceGeneratedJsonContext : JsonSerializerContext
{
}