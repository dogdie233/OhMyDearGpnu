using OhMyDearGpnu;
using OhMyDearGpnu.Api;
using OhMyDearGpnu.Api.AcaAff;
using OhMyDearGpnu.Api.AcaAff.Requests;
using OhMyDearGpnu.Api.Cas;
using OhMyDearGpnu.Api.IoT;

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

var acaAff = client.GetAcaAffContext();
Console.WriteLine("正在获取个人信息");
var personInfoResponse = await client.SendRequest(new PersonInfoRequest());
Console.WriteLine($"你好，{personInfoResponse.Name}({personInfoResponse.StudentID})");

Console.WriteLine("正在获取日历");
var calendar = await acaAff.GetCalendar();
Console.WriteLine($"本周为第{calendar.CurrentWeek}周");
var curriculums = await acaAff.GetCurriculums(2023, "3");
Console.WriteLine($"========本周的课表为========");
foreach (var curriculum in curriculums)
{
    if (!curriculum.Week.IsInRange(calendar.CurrentWeek))
        continue;
    Console.Write($"周{curriculum.Day}    ");
    Console.Write($"{curriculum.Name.PadRight(20, ' ')}");
    Console.Write($"{curriculum.Classroom.PadRight(10, ' ')}");
    Console.WriteLine();
}

Console.WriteLine("============================");