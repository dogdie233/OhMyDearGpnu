using System.Text.Json.Serialization;

namespace OhMyDearGpnu.Api.StuAff.Models;

public class UserInfoModel
{
    [JsonPropertyName("userId")] public string UserId { get; set; } = string.Empty;
    [JsonPropertyName("userName")] public string UserName { get; set; } = string.Empty;
    [JsonPropertyName("userNickname")] public string? UserNickname { get; set; }
    [JsonPropertyName("sortNum")] public float SortNum { get; set; }
    [JsonPropertyName("qq")] public string? QQ { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("kindCode")] public string? KindCode { get; set; }
    [JsonPropertyName("studentUrl")] public string? StudentUrl { get; set; }
    [JsonPropertyName("teacherUrl")] public string? TeacherUrl { get; set; }
    [JsonPropertyName("firstLogin")] public bool FirstLogin { get; set; }
    [JsonPropertyName("orgId")] public string OrgId { get; set; } = string.Empty;
    [JsonPropertyName("departmentId")] public string DepartmentId { get; set; } = string.Empty;
    [JsonPropertyName("departmentName")] public string DepartmentName { get; set; } = string.Empty;
    [JsonPropertyName("userType")] public string UserType { get; set; } = string.Empty;
    [JsonPropertyName("sex")] public string Sex { get; set; } = string.Empty;
    [JsonPropertyName("skin")] public string? Skin { get; set; }
    [JsonPropertyName("tokenId")] public string TokenId { get; set; } = string.Empty;
    [JsonPropertyName("host")] public string Host { get; set; } = string.Empty;
    [JsonPropertyName("runAs")] public bool RunAs { get; set; }
}