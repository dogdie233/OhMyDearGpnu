using OhMyDearGpnu;
using OhMyDearGpnu.Api;
using OhMyDearGpnu.Api.AcaAff;
using OhMyDearGpnu.Api.AcaAff.Requests;
using OhMyDearGpnu.Api.Cas;
using OhMyDearGpnu.Api.IoT;
using OhMyDearGpnu.Api.TeachEval;

var client = new GpnuClient();

var casHandler = client.cas;
string? username = null, password = null;


Console.WriteLine("请选择登录方式：");
Console.WriteLine("1. TGT登录");
Console.WriteLine("2. 账号密码登录");
Console.WriteLine("3. 微信扫码登录");
Console.Write("请输入选项(1-3): ");

var choice = Console.ReadLine();

switch (choice)
{
    case "1":
        await LoginByTgt(casHandler);
        break;
    case "2":
        await LoginByPassword(casHandler);
        break;
    case "3":
        await LoginByWechat(casHandler);
        break;
    default:
        Console.WriteLine("无效选项，程序退出");
        return;
}

Console.WriteLine($"cas 登录成功，TGT为{casHandler.Tgt}");

var iot = client.GetIoTContext();
var myRooms = await iot.ListMyRoomElectric();
var roomCode = myRooms[0].RoomCode;
var electricBalance = await iot.GetElectricBalance(roomCode);
Console.WriteLine($"你的房间是：{roomCode}，电费余额为{electricBalance.MoneyBalance}");

var teachContext = client.GetTeachEvalContext();
var unfinished = await teachContext.GetMyTaskItemByAnswerStatus();
Console.WriteLine($"你有{unfinished.TotalCount}个未完成的教学评价任务");



async Task LoginByTgt(CasHandler casHandler)
{
    Console.WriteLine("正在使用TGT登录");
    Console.Write("请输入TGT：");
    await casHandler.LoginByTgt(Console.ReadLine()!);
}

async Task LoginByPassword(CasHandler casHandler)
{
    Console.Write("请输入学号: ");
    username = Console.ReadLine()!;
    Console.Write("请输入密码: ");
    password = Helper.ReadPassword();

    var casCaptcha = await casHandler.GetPasswordLoginCaptcha();
    File.Delete("casCaptcha.png");
    using (var fs = File.Create("casCaptcha.png"))
    {
        await fs.WriteAsync(casCaptcha.image);
    }

    Console.WriteLine("已将验证码保存至casCaptcha.png");
    Console.Write("请输入验证码结果（留空则自动填充）: ");
    casCaptcha.value = Console.ReadLine();
    if (string.IsNullOrEmpty(casCaptcha.value))
        casCaptcha.value = null;
    try
    {
        var casLoginRes = await casHandler.LoginByPassword(username, password, casCaptcha, true);
    }
    catch (CasLoginFailException e)
    {
        Console.WriteLine($"cas 登录失败，原因：{e.Message}");
        throw;
    }
}

async Task LoginByWechat(CasHandler casHandler)
{
    try
    {
        var casLoginRes = await casHandler.LoginByWechat();
    }
    catch (CasLoginFailException e)
    {
        Console.WriteLine($"cas 登录失败，原因：{e.Message}");
        throw;
    }
}