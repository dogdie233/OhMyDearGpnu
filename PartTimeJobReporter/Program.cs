using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using OhMyDearGpnu.Api;
using OhMyDearGpnu.Api.StuAff;
using OhMyDearGpnu.Api.StuAff.Models;
using OhMyDearGpnu.Api.StuAff.PartTimeJob.Models;
using OhMyDearGpnu.Api.StuAff.PartTimeJob.Requests;

using Spectre.Console;

RegistrationPiece[] registrationPieces =
[
    new RegistrationPiece(new TimeOnly(8, 20), new TimeOnly(9, 50), 2),
    new RegistrationPiece(new TimeOnly(10, 0), new TimeOnly(12, 0), 2),
    new RegistrationPiece(new TimeOnly(14, 0), new TimeOnly(15, 0), 1),
    new RegistrationPiece(new TimeOnly(15, 0), new TimeOnly(17, 0), 2)
];

var client = new GpnuClient();
var http = new HttpClient();

#region Parse Args

var noInteractive = args.Contains("--no-ia");

#endregion

#region Load Config

var configFilePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath) ?? Environment.CurrentDirectory, "config.json");
Config? config = null;
if (File.Exists(configFilePath))
{
    var configJson = await File.ReadAllTextAsync(configFilePath);
    try
    {
        config = JsonSerializer.Deserialize<Config>(configJson, SourceGeneratedJsonContext.Default.Config);
    }
    catch (JsonException ex)
    {
        LogError("配置文件解析失败，删掉！");
        AnsiConsole.WriteException(ex);
        File.Delete(configFilePath);
    }
}

if (config == null)
{
    LogWarning("配置文件无效，生成一个");
    config = new Config();
    if (!noInteractive)
    {
        LogInfo("开始配置向导");
        config.Username = AnsiConsole.Ask<string>("请输入你的用户名(cas): ");
        config.Password = AnsiConsole.Ask<string>("请输入你的密码(cas): ");

        List<MyJobModel>? jobs = null;

        try
        {
            await AnsiConsole.Status()
                .StartAsync("正在登录...", async ctx =>
                {
                    ctx.Status("正在获取登录验证码...");
                    var captcha = await client.cas.GetPasswordLoginCaptcha();
                    AnsiConsole.MarkupLine("获取登录验证码[green]成功[/]");

                    ctx.Status("正在尝试登录...");
                    _ = await client.cas.LoginByPassword(config.Username, config.Password, captcha);
                    AnsiConsole.MarkupLine("登录[green]成功[/]");

                    var stuAffContext = client.GetStuAffContext();

                    ctx.Status("正在获取你的勤工俭学岗位列表...");
                    jobs = (await stuAffContext.GpnuClient.SendRequest(new QueryMyJobForWorkloadRegistrationRequest(stuAffContext.UserInfo.UserId)
                    {
                        Pagination = new PaginationModel(1, 114514)
                    })).List;
                    AnsiConsole.MarkupLine("获取你的勤工俭学岗位列表[green]成功[/]");
                });
        }
        catch (Exception ex)
        {
            LogError("发生意料之外的错误");
            AnsiConsole.WriteException(ex);
            Exit();
        }

        if (jobs is not { Count: > 0 })
        {
            LogError("你没有勤工俭学岗位，无法继续");
            Exit(true);
        }

        var job = AnsiConsole.Prompt(
            new SelectionPrompt<JobOption>()
                .Title("请选择你要报工时的岗位")
                .PageSize(10)
                .MoreChoicesText("更多")
                .AddChoices(jobs.Select(model => new JobOption(model)))
        ).Job;

        config.JobId = job.JobId;
        AnsiConsole.MarkupLine("配置向导[green]完成[/]，下次运行将自动使用这个配置");
    }
    else
    {
        LogError("配置文件无效，重新生成");
    }

    await WriteConfigAsync(configFilePath, config);
    LogInfo($"写入配置文件到{configFilePath}");
    Exit(true);
}

#endregion

#region Do Action

try
{
    LogInfo("获取验证码中...");
    var captcha = await client.cas.GetPasswordLoginCaptcha();

    LogInfo("登录中...");
    _ = await client.cas.LoginByPassword(config.Username, config.Password, captcha);
    var stuAff = client.GetStuAffContext();

    var dateNow = DateTime.Now;
    LogInfo($"现在是 {dateNow:yyyy-MM}, 获取节假日中...");
    var holidayRequest = await http.GetAsync("https://api.comm.miui.com/holiday/holiday.jsp");
    var holidays = (await holidayRequest.Content.ReadFromJsonAsync(SourceGeneratedJsonContext.Default.CalendarModel))?.Holiday.FirstOrDefault(h => h.Year == dateNow.Year);
    if (holidays == null)
    {
        LogError("节假日信息获取失败");
        Exit();
    }

    LogInfo("正在获取已报时长");
    var months = (await client.SendRequest(new QuerySalaryRequest(config.JobId.ToString(), stuAff.UserInfo.UserId)
    {
        Pagination = new PaginationModel(1, 114514)
    })).List;
    var month = months.FirstOrDefault(data => data.Date.Year == dateNow.Year && data.Date.Month == dateNow.Month);
    var requiredHour = config.RequiredHour - (month?.WorkHours ?? 0);
    LogInfo($"本月需报时长为 [yellow]{config.RequiredHour}[/] 小时，已报 [yellow]{month?.WorkHours ?? 0}[/] 小时，还需报 [yellow]{requiredHour}[/] 小时");
    if (requiredHour <= 0)
    {
        LogInfo("已完成本月报工，结束");
        Exit(true);
    }

    var restDays = holidays.FreeDays.Select(day => new DateOnly(dateNow.Year, 1, 1).AddDays(day - 1)).ToArray();
    var availableDays = Enumerable.Range(1, DateTime.DaysInMonth(dateNow.Year, dateNow.Month))
        .Select(day => new DateOnly(dateNow.Year, dateNow.Month, day))
        .Where(date => date is { DayOfWeek: >= DayOfWeek.Monday and <= DayOfWeek.Friday })
        .Except(restDays)
        .ToList();
    LogInfo("将会在以下日期报工时：" + string.Join(", ", availableDays.Select(date => date.ToString("yyyy-MM-dd"))));
    var walked = new bool[availableDays.Count, registrationPieces.Length];
    var walkedHours = 0;

    bool Dfs()
    {
        if (walkedHours >= requiredHour) return true;
        var remainHour = requiredHour - walkedHours;

        while (walked.Cast<bool>().Any(b => !b))
        {
            int day, piece;
            do
            {
                day = Random.Shared.Next(0, walked.GetLength(0));
                piece = Random.Shared.Next(0, walked.GetLength(1));
            } while (walked[day, piece] || remainHour < registrationPieces[piece].WorkHours);

            walked[day, piece] = true;
            walkedHours += registrationPieces[piece].WorkHours;
            if (walkedHours == requiredHour || Dfs()) return true;
            walked[day, piece] = false;
            walkedHours -= registrationPieces[piece].WorkHours;
        }

        throw new InvalidOperationException("无解");
    }

    Dfs();

    var items = new List<(DateOnly day, int pieceIdx)>();
    for (var day = 0; day < walked.GetLength(0); day++)
    for (var piece = 0; piece < walked.GetLength(1); piece++)
        if (walked[day, piece])
            items.Add((availableDays[day], piece));

    LogInfo("正在登录到勤工俭学系统...");
    foreach (var item in items)
    {
        LogInfo("添加条目：" + item);
        var request = PostWorkloadItemRequest.CreateInsertRequest(stuAff.Token, stuAff.UserInfo.UserId, config.JobId, item.day, registrationPieces[item.pieceIdx].StartTime, registrationPieces[item.pieceIdx].EndTime, registrationPieces[item.pieceIdx].WorkHours);
        await stuAff.GpnuClient.SendRequest(request);
    }

    Exit(true);
}
catch (Exception e)
{
    LogError("出现意料之外的错误，结束");
    AnsiConsole.WriteException(e);
    Exit();
}

#endregion

[UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
[UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
static async Task WriteConfigAsync(string path, Config config)
{
    await using var fs = File.Create(path);
    await JsonSerializer.SerializeAsync(fs, config, options: new JsonSerializerOptions(JsonSerializerOptions.Default)
    {
        TypeInfoResolver = SourceGeneratedJsonContext.Default,
        WriteIndented = true
    });
}

void LogInfo(string message)
{
    AnsiConsole.MarkupLine($"[[{DateTime.Now.ToLongTimeString()}]][[info]]: {message}");
}

void LogWarning(string message)
{
    AnsiConsole.MarkupLine($"[yellow][[{DateTime.Now.ToLongTimeString()}]][[warn]]: {message}[/]");
}

void LogError(string message)
{
    AnsiConsole.MarkupLine($"[red][[{DateTime.Now.ToLongTimeString()}]][[error]]: {message}[/]");
}

[DoesNotReturn]
void Exit(bool success = false)
{
    if (!noInteractive)
    {
        AnsiConsole.Markup("[green]程序已结束，按任意键关闭[/]");
        Console.ReadKey();
    }

    Environment.Exit(success ? 0 : 1);
}

internal class Config
{
    public string Username { get; set; } = "这是你的用户名(cas)";
    public string Password { get; set; } = "这是你的密码(cas)";
    public Guid JobId { get; set; }
    public int RequiredHour { get; set; } = 30;
}

internal readonly record struct JobOption(MyJobModel Job)
{
    public override string ToString()
    {
        return $"[green]{Job.Status}[/] [yellow]{Job.WorkYearName}[/] {Job.JobName} ({Job.JobNatureName})";
    }
}

internal readonly record struct RegistrationPiece(TimeOnly StartTime, TimeOnly EndTime, int WorkHours);

internal class CalendarModel
{
    [JsonPropertyName("versioncode")] public int VersionCode { get; set; }
    [JsonPropertyName("holiday")] public List<HolidayModel> Holiday { get; set; } = [];
}

internal class HolidayModel
{
    [JsonPropertyName("year")] public int Year { get; set; }
    [JsonPropertyName("workday")] public int[] Workdays { get; set; } = [];
    [JsonPropertyName("freeday")] public int[] FreeDays { get; set; } = [];
}

[JsonSerializable(typeof(Config))]
[JsonSerializable(typeof(CalendarModel))]
partial class SourceGeneratedJsonContext : JsonSerializerContext {}