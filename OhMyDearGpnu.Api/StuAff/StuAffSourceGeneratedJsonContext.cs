using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.StuAff.Models;
using OhMyDearGpnu.Api.StuAff.PartTimeJob.Models;
using OhMyDearGpnu.Api.StuAff.PartTimeJob.Requests;
using OhMyDearGpnu.Api.StuAff.Responses;

namespace OhMyDearGpnu.Api.StuAff;

[JsonSerializable(typeof(PaginationModel))]
[JsonSerializable(typeof(UserInfoModel))]
[JsonSerializable(typeof(GridListSignsModel))]
[JsonSerializable(typeof(MonthlySalaryModel))]
[JsonSerializable(typeof(MyJobModel))]
[JsonSerializable(typeof(WorkloadItemModel))]
[JsonSerializable(typeof(DeleteWorkloadItemRequest))]
[JsonSerializable(typeof(JsonResponseModel<int?>))]
[JsonSerializable(typeof(PostWorkloadItemRequest))]
[JsonSerializable(typeof(JsonResponseModel<int?>))]
[JsonSerializable(typeof(QueryMyJobForWorkloadRegistrationRequest))]
[JsonSerializable(typeof(JsonResponseModel<Paged<MyJobModel>>))]
[JsonSerializable(typeof(QueryWorkloadRegistrationsRequest))]
[JsonSerializable(typeof(JsonResponseModel<Paged<WorkloadItemModel>>))]
[JsonSerializable(typeof(QuerySalaryRequest))]
[JsonSerializable(typeof(JsonResponseModel<Paged<MonthlySalaryModel>>))]
[JsonSerializable(typeof(QueryGridListSignsRequest))]
[JsonSerializable(typeof(JsonResponseModel<GridListSignsModel>))]
[JsonSerializable(typeof(QueryMyJobRequest))]
[JsonSerializable(typeof(JsonResponseModel<Paged<MyJobModel>>))]
[JsonSerializable(typeof(GetUserInfoResponse))]
public partial class StuAffSourceGeneratedJsonContext : JsonSerializerContext { }