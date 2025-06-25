using System.Text.Json.Serialization;

using OhMyDearGpnu.Api.Serializer.Json;

namespace OhMyDearGpnu.Api.TeachEval.Models;

public class AuthorityItem
{
    [JsonPropertyName("RightsCode")] public string RightsCode { get; set; } = string.Empty;

    [JsonPropertyName("RightsStatus")] public int RightsStatus { get; set; }
}

public class UserInfoModel
{
    [JsonPropertyName("Password")] public string Password { get; set; } = string.Empty;

    [JsonPropertyName("Name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("ID")] public int Id { get; set; }

    [JsonPropertyName("Code")] public string Code { get; set; } = string.Empty;

    [JsonPropertyName("RoleID")] public int RoleId { get; set; }

    [JsonPropertyName("RoleName")] public string RoleName { get; set; } = string.Empty;

    [JsonPropertyName("LoginTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime LoginTime { get; set; }

    [JsonPropertyName("DepartmentCode")] public string DepartmentCode { get; set; } = string.Empty;

    [JsonPropertyName("DepartmentName")] public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("IsTeachingGroupMaster")]
    public bool IsTeachingGroupMaster { get; set; }

    [JsonPropertyName("CurrentSemester")] public string CurrentSemester { get; set; } = string.Empty;

    [JsonPropertyName("IsEncrypted")] public bool IsEncrypted { get; set; }

    [JsonPropertyName("UniversityCode")] public string UniversityCode { get; set; } = string.Empty;

    [JsonPropertyName("MajorName")] public string MajorName { get; set; } = string.Empty;

    [JsonPropertyName("Authority")] public List<AuthorityItem> Authority { get; set; } = [];

    [JsonPropertyName("AuthOutAddress")] public string AuthOutAddress { get; set; } = string.Empty;

    [JsonPropertyName("deployType")] public int DeployType { get; set; }

    [JsonPropertyName("IsExpertOrSupervisor")]
    public bool IsExpertOrSupervisor { get; set; }

    [JsonPropertyName("DegreeLevel")] public int DegreeLevel { get; set; }

    [JsonPropertyName("SchoolDegreeLevel")]
    public int SchoolDegreeLevel { get; set; }

    [JsonPropertyName("ResetPwdAndEmail")] public bool ResetPwdAndEmail { get; set; }

    [JsonPropertyName("Qualifications")] public int Qualifications { get; set; }

    [JsonPropertyName("StudentRole")] public int StudentRole { get; set; }

    [JsonPropertyName("MenuRoleId")] public int MenuRoleId { get; set; }

    [JsonPropertyName("LockDateTime")]
    [JsonConverter(typeof(TeachEvalDateTimeConverter))]
    public DateTime LockDateTime { get; set; }

    [JsonPropertyName("LoginFailCount")] public int LoginFailCount { get; set; }

    [JsonPropertyName("NewSemesterStatistic")]
    public string NewSemesterStatistic { get; set; } = string.Empty;

    [JsonPropertyName("currentAuthority")] public int CurrentAuthority { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;

    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;

    [JsonPropertyName("Token")] public string Token { get; set; } = string.Empty;
}