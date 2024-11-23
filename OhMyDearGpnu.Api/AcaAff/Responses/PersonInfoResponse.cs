namespace OhMyDearGpnu.Api.AcaAff.Responses;

public class PersonInfoResponse
{
    [Alias("学号")] public string? StudentID { get; set; }
    [Alias("姓名")] public string? Name { get; set; }
    [Alias("姓名拼音")] public string? NamePinYin { get; set; }
    [Alias("曾用名")] public string? FormerName { get; set; }
    [Alias("英文姓名")] public string? EnglishName { get; set; }
    [Alias("性别")] public string? Gender { get; set; }
    [Alias("证件类型")] public string? IDType { get; set; }
    [Alias("证件号码")] public string? IDNumber { get; set; }
    [Alias("出生日期")] public DateTime? BirthDate { get; set; }
    [Alias("民族")] public string? Nation { get; set; }
    [Alias("政治面貌")] public string? PoliticalStatus { get; set; }
    [Alias("政治面貌加入时间")] public DateTime? PoliticalStatusJoinedDate { get; set; }
    [Alias("入学时间")] public DateTime? RegisterDate { get; set; }
    [Alias("籍贯")] public string? NativePlace { get; set; } // 籍贯
    [Alias("户口所在地")] public string? DomicilePlace { get; set; } // 户口所在地
    [Alias("生源地")] public string? DomicilePlaceWhenGaokao { get; set; } // 生源地
    [Alias("出生地")] public string? BirthPlace { get; set; } // 出生地

    [Alias("血型名称")] public string? BloodType { get; set; }

    // [Alias("港澳台侨外")] public string? 港澳台侨外 { get; set; }
    [Alias("国籍/地区")] public string? Nationality { get; set; }
    [Alias("学生类型")] public string? StudentType { get; set; }
}