using System.Diagnostics.CodeAnalysis;

using OhMyDearGpnu.Api;
using OhMyDearGpnu.Api.Cas;
using OhMyDearGpnu.Api.TeachEval;
using OhMyDearGpnu.Api.TeachEval.Models;
using OhMyDearGpnu.Api.TeachEval.Requests;

using Spectre.Console;

var ansCache = new Dictionary<int, (List<SubjectAnswerModel>, int)>();

var tgt = AnsiConsole.Ask<string>("请输入tgt(没有则输入n): ");
string? username = null;
string? password = null;
if (tgt == "n")
{
    username = AnsiConsole.Ask<string>("请输入你的用户名(cas): ");
    password = AnsiConsole.Ask<string>("请输入你的密码(cas): ");
    if (username is not { Length: > 0 } || password is not { Length: > 0 })
        ErrExit("用户名或密码不能为空");
}

var selectionMode = AnsiConsole.Prompt(new SelectionPrompt<SelectionModePrompt>()
        .Title("问卷的选择题处理方式")
        .PageSize(10)
        .AddChoices(new SelectionModePrompt(SelectionMode.HighScore, "自动选择高分选项"), new SelectionModePrompt(SelectionMode.Manual, "手动")))
    .Mode;
var holdingText = AnsiConsole.Ask<string>("填空题默认填什么：", "老师人很好，下次还跟");
var delayFinish = AnsiConsole.Confirm("是否在两次问卷提交之间添加间隔");

try
{
    await AnsiConsole.Status()
        .StartAsync("正在登录...", async ctx =>
        {
            var client = new GpnuClient();
            if (tgt != "n" && tgt != "")
            {
                await client.cas.LoginByTgt(tgt);
            }
            else if (username != null && password != null)
            {
                var captcha = await client.cas.GetPasswordLoginCaptcha();
                await client.cas.LoginByPassword(username, password, captcha);
            }

            AnsiConsole.MarkupLine("Cas登录成功");

            var te = client.GetTeachEvalContext();
            AnsiConsole.MarkupLine("教育质量评价系统登录成功");

            ctx.Status = "正在获取任务列表...";
            var taskItems = (await te.GetMyTaskItemByAnswerStatus()).Items;

            AnsiConsole.MarkupLine($"获取到 [yellow]{taskItems.Count}[/] 个任务项");
            foreach (var taskItem in taskItems)
            {
                var url = QuestionnairePageUrl.CreateFromTaskItem(taskItem);
                AnsiConsole.MarkupLine($"正在获取任务项 [yellow]{taskItem.QuestionnaireName}[/] 的课程列表");
                while (true)
                {
                    var taskItemDetails = (await client.SendRequest(new GetMyTaskItemDetailRequest(taskItem.TaskId))).Items;
                    if (taskItemDetails is null or { Count: 0 })
                        break;

                    Console.WriteLine($"获取到 {taskItemDetails.Count} 个课程项");
                    foreach (var taskItemDetail in taskItemDetails)
                    {
                        url = url.With(taskItemDetail);
                        ctx.Status = $"正在获取课程 [yellow]{taskItemDetail.CourseName}[/] 的信息";
                        var header = await client.SendRequest(new GetFinalQuestionnaireHeaderRequest(url, taskItemDetail.PersonCode, taskItemDetail.CourseCode));
                        var questionnaire = await client.SendRequest(new GetQuestionnaireRequest(url));

                        ctx.Status = "正在处理问卷";
                        var teachers = header.CourseList.SelectMany(c => c.TeacherList);
                        foreach (var teacher in teachers)
                        {
                            var (answer, second) = SolveQuestionnaire(questionnaire);
                            var success = await client.SendRequest(new SaveQuestionnaireAnswerRequest(url)
                            {
                                DetailId = teacher.DetailId,
                                Version = header.Version,
                                PersonCode = te.UserInfo.Code,
                                TotalAnsweredSecond = second,
                                Subjects = answer
                            });
                            AnsiConsole.MarkupLine(!success
                                ? $"[red]N[/]教师：{teacher.TeacherName}({teacher.TeacherCode})的问卷({teacher.DetailId})提交失败"
                                : $"[green]Y[/]教师：{teacher.TeacherName}({teacher.TeacherCode})的问卷({teacher.DetailId})提交成功");

                            if (delayFinish)
                            {
                                AnsiConsole.MarkupLine($"等待 {second} 秒后继续下一个问卷");
                                await Task.Delay(second);
                            }
                        }
                    }
                }
            }
        });

    AnsiConsole.WriteLine("所有问卷已提交完成");
}
catch (CasLoginFailException ex)
{
    ErrExit($"登录失败：{ex.Message}");
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex);
    ErrExit("由于未知的致命错误，程序无法继续运行");
    throw;
}

return;

(List<SubjectAnswerModel> answer, int second) SolveQuestionnaire(QuestionnaireModel questionnaire)
{
    if (ansCache.TryGetValue(questionnaire.Id, out var value))
    {
        value.Item2 += Random.Shared.Next(-value.Item2 / 2, value.Item2 / 2);
        return value;
    }

    var sortedItems = questionnaire.PositionOrder is { Count: > 0 }
        ? questionnaire.PositionOrder.Select(id => questionnaire.Items.FirstOrDefault(i => i.Id == id)
                                                   ?? throw new InvalidOperationException($"找不到ID为{id}的问卷项")).ToList()
        : questionnaire.Items;
    var answer = new List<SubjectAnswerModel>();
    var startTime = DateTime.Now;

    AnsiConsole.WriteLine($"你需要先手动完成一次此问卷({questionnaire.Id})");
    foreach (var item in sortedItems)
        switch (item.SubjectType)
        {
            case 1: // 单选题
            {
                var ans = selectionMode switch
                {
                    SelectionMode.HighScore => item.Props.Items.Options.OrderByDescending(o => o.BandScore).FirstOrDefault()?.Title ?? throw new InvalidOperationException("没有可选的选项"),
                    SelectionMode.Manual => AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"{item.Title} (单选)").PageSize(10).AddChoices(item.Props.Items.Options.Select(o => o.Title))),
                    _ => throw new InvalidOperationException($"不支持的选择题处理方式：{selectionMode}")
                };

                AnsiConsole.MarkupLine($"{item.Title}: {ans}");
                answer.Add(new SubjectAnswerModel
                {
                    SubjectId = item.Props.SubjectId,
                    SubjectItems =
                    [
                        new SubjectAnswerItemModel
                        {
                            OptionId = item.Props.Items.Options.First(o => o.Title == ans).Id
                        }
                    ]
                });
                break;
            }
            case 4:
            {
                AnsiConsole.MarkupLine($"{item.Title}: {holdingText}");
                answer.Add(new SubjectAnswerModel
                {
                    SubjectId = item.Props.SubjectId,
                    SubjectItems =
                    [
                        new SubjectAnswerItemModel
                        {
                            OptionId = item.Props.Items.Options.First().Id,
                            ItemValue = holdingText
                        }
                    ]
                });
                break;
            }
            default:
                throw new InvalidOperationException($"不支持的问卷项类型：{item.SubjectType}，找开发者（");
        }

    var endTime = DateTime.Now;
    var second = (int)(endTime - startTime).TotalSeconds;
    ansCache[questionnaire.Id] = (answer, second);
    return (answer, second);
}

[DoesNotReturn]
static void ErrExit(string msg)
{
    Console.WriteLine($"错误：{msg}");
    Console.WriteLine("程序已结束运行，按回车关闭窗口");
    Console.ReadKey();
    Environment.Exit(1);
}

internal enum SelectionMode
{
    HighScore,
    Manual
}

internal readonly record struct SelectionModePrompt(SelectionMode Mode, string Title)
{
    public override string ToString()
    {
        return Title;
    }
}