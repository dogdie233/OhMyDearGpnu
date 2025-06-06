﻿using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.StuAff.PartTimeJob.Models;

namespace OhMyDearGpnu.Api.StuAff.PartTimeJob.Requests;

[Request(PayloadTypeEnum.Json)]
public partial class QueryMyJobForWorkloadRegistrationRequest : QueryGridListRequest<MyJobModel>
{
    public static readonly string presetSign = "zhxg_workStudy_djgzl_wdgw_table";

    public QueryMyJobForWorkloadRegistrationRequest(string token, string studentId) : base(token, presetSign)
    {
        SqlParams.Add("loginUserId", studentId);
    }
}

[Request(PayloadTypeEnum.Json)]
public partial class QueryWorkloadRegistrationsRequest : QueryGridListRequest<WorkloadItemModel>
{
    public static readonly string presetSign = "zhxg_workStudy_djgzl_mx_table";

    public QueryWorkloadRegistrationsRequest(string token, string jobId, string studentId) : base(token, presetSign)
    {
        SqlParams.Add("gwxxid", jobId);
        SqlParams.Add("xsid", studentId);
    }
}

[Request(PayloadTypeEnum.Json)]
public partial class QuerySalaryRequest : QueryGridListRequest<MonthlySalaryModel>
{
    public static readonly string presetSign = "zhxg_workStudy_djgzl_xctj_table";

    public QuerySalaryRequest(string token, string jobId, string studentId) : base(token, presetSign)
    {
        SqlParams.Add("gwxxid", jobId);
        SqlParams.Add("xsid", studentId);
    }
}