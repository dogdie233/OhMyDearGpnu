﻿using OhMyDearGpnu;
using OhMyDearGpnu.Api;
using OhMyDearGpnu.Api.AcaAff;
using OhMyDearGpnu.Api.AcaAff.Requests;
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
    Console.Write("请输入验证码结果: ");
    casCaptcha.value = Console.ReadLine();
    var casLoginRes = await casHandler.LoginByPassword(username, password, casCaptcha, true);
    if (!casLoginRes.IsSucceeded)
    {
        Console.WriteLine($"cas 登录失败，原因：{casLoginRes.ErrorMessage}");
        return;
    }
}

Console.WriteLine($"cas 登录成功，TGT为{casHandler.Tgt}");

var iot = client.GetIoTContext();
var myRooms = await iot.ListMyRoomElectric();
var roomCode = myRooms.data![0].RoomCode;
var electricBalance = await iot.GetElectricBalance(roomCode);
Console.WriteLine($"你的房间是：{roomCode}，电费余额为{electricBalance.data!.MoneyBalance}");

if (username == null || password == null)
{
    Console.Write("请输入学号: ");
    username = Console.ReadLine()!;
    Console.Write("请输入密码: ");
    password = Helper.ReadPassword();
}

Console.WriteLine("正在获取验证码");
var captcha = (await client.AcaAffGetCaptcha()).data!;
File.Delete("captcha.jpg");
using (var fs = File.Create("captcha.jpg"))
{
    await captcha.imageStream.CopyToAsync(fs);
}

Console.WriteLine("已将验证码保存至captcha.jpg");
Console.Write("请输入验证码: ");
captcha.value = Console.ReadLine();
var loginRes = await client.AcaAffLogin(username, password, captcha);
if (loginRes.IsSucceeded)
{
    Console.WriteLine("登录成功，正在获取个人信息");
    var personInfoResponse = await client.SendRequest(new PersonInfoRequest());
    if (personInfoResponse.IsSucceeded)
    {
        Console.WriteLine($"你好，{personInfoResponse.data!.Name}({personInfoResponse.data.StudentID})");
    }
    else
    {
        Console.WriteLine($"获取个人信息失败");
        Environment.Exit(0);
    }

    Console.WriteLine("正在获取日历");
    var calendarRes = await client.AcaAffGetCalendar();
    var calendar = calendarRes.data!;
    Console.WriteLine($"本周为第{calendar.CurrentWeek}周");
    var curriculumsRes = await client.AcaAffGetCurriculums(2023, "3");
    var curriculums = curriculumsRes.data!;
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
}
else
{
    Console.WriteLine($"登录失败，{loginRes.message}");
}