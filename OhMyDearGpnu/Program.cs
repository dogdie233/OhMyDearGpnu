﻿using OhMyDearGpnu;
using OhMyDearGpnu.Api;
using OhMyDearGpnu.Api.Requests;

var client = new GpnuClient();
Console.Write("请输入学号: ");
var username = Console.ReadLine();
Console.Write("请输入密码: ");
var password = Helper.ReadPassword();

Console.WriteLine("正在获取验证码");
var captcha = (await client.GetCaptcha()).data;
File.Delete("captcha.jpg");
using (var fs = File.Create("captcha.jpg"))
{
    await captcha.imageStream.CopyToAsync(fs);
    captcha.imageStream.Dispose();
}
Console.WriteLine("已将验证码保存至captcha.jpg");
Console.Write("请输入验证码: ");
captcha.value = Console.ReadLine();
var loginRes = await client.Login(username, password, captcha);
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
    var calendarRes = await client.GetCalendar();
    var calendar = calendarRes.data!;
    Console.WriteLine($"本周为第{calendar.CurrentWeek}周");
    var curriculumsRes = await client.GetCurriculums(2023, "3");
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
    Console.WriteLine($"登录失败，{loginRes.message}");