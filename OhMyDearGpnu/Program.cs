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
Console.Write("需要通过cas认证，是否使用TGT登录(y/n)：");
if (Console.ReadLine() == "y")
{
    Console.WriteLine("正在使用TGT登录");
    Console.Write("请输入TGT：");
    await casHandler.LoginByTgt(Console.ReadLine()!);
}
else
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

Console.WriteLine($"cas 登录成功，TGT为{casHandler.Tgt}");

var iot = client.GetIoTContext();
var myRooms = await iot.ListMyRoomElectric();
var roomCode = myRooms[0].RoomCode;
var electricBalance = await iot.GetElectricBalance(roomCode);
Console.WriteLine($"你的房间是：{roomCode}，电费余额为{electricBalance.MoneyBalance}");

var teachContext = client.GetTeachEvalContext();
var unfinished = await teachContext.GetMyTaskItemByAnswerStatus();
Console.WriteLine($"你有{unfinished.TotalCount}个未完成的教学评价任务");