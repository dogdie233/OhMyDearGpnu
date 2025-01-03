using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Serializer.Json;
using OhMyDearGpnu.Api.StuAff.Requests;

namespace OhMyDearGpnu.Api.StuAff.PartTimeJob.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class PostWorkloadItemRequest(string token) : JsonApiRequestBase<int?>(token)
{
    public override Uri Url => new(Hosts.stuAff, "qgzx/api/sm-work-study/jobRegister/insertOrUpdate");
    public override HttpMethod HttpMethod => HttpMethod.Post;

    [JsonPropertyName("gzlid")] public Guid? RegistrationId { get; set; }
    [JsonPropertyName("xsid")] public required string StudentId { get; set; } = string.Empty;
    [JsonPropertyName("gwxxid")] public required Guid JobId { get; set; }
    [JsonPropertyName("gzrq")] public required DateOnly WorkDate { get; set; }

    [JsonPropertyName("kssj")]
    [JsonConverter(typeof(TimeOnlyHourMinuteConverter))]
    public required TimeOnly StartTime { get; set; }

    [JsonPropertyName("jssj")]
    [JsonConverter(typeof(TimeOnlyHourMinuteConverter))]
    public required TimeOnly EndTime { get; set; }

    [JsonPropertyName("gzsc")] public required int WorkHours { get; set; }

    public static PostWorkloadItemRequest CreateInsertRequest(string token, string studentId, Guid jobId, DateOnly workDate, TimeOnly startTime, TimeOnly endTime, int workHours)
    {
        return new PostWorkloadItemRequest(token)
        {
            StudentId = studentId,
            JobId = jobId,
            WorkDate = workDate,
            StartTime = startTime,
            EndTime = endTime,
            WorkHours = workHours
        };
    }

    public static PostWorkloadItemRequest CreateUpdateRequest(string token, Guid registrationId, string studentId, Guid jobId, DateOnly workDate, TimeOnly startTime, TimeOnly endTime, int workHours)
    {
        return new PostWorkloadItemRequest(token)
        {
            RegistrationId = registrationId,
            StudentId = studentId,
            JobId = jobId,
            WorkDate = workDate,
            StartTime = startTime,
            EndTime = endTime,
            WorkHours = workHours
        };
    }
}