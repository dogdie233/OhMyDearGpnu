# Oh My Dear Gpnu
一个 _广东技术师范大学教务管理系统_ 的后端API Wrapper  
__本项目并非广东技术师范大学官方的项目__  
> 只有一点点的性能

# 使用方法

1. 添加OhMyDearGpnu.Api至项目引用
2. 享用接口吧

```CSharp
var client = new GpnuClient();  // 创建一个客户端实例
var casHandler = client.cas;
var captcha = await casHandler.GetPasswordLoginCaptcha();
var useCaptchaResolver = true;  // 使用内置的验证码识别器（实验性功能）
captcha.value = useCaptchaResolver ? null : "114514";  // 置null则使用内置的验证码识别器

try
{
    var casLoginRes = await casHandler.LoginByPassword(username, password, casCaptcha, true);
    Console.WriteLine($"cas 登录成功，TGT为{casLoginRes.tgt}");
}
catch (CasLoginFailException e)
{
    Console.WriteLine($"cas 登录失败，原因：{e.Message}");
    throw;
}

var acaAff = client.GetAcaAffContext();
Console.WriteLine("正在获取个人信息");
var personInfoResponse = await client.SendRequest(new PersonInfoRequest());  // 使用new一个Request的方式发送请求
Console.WriteLine($"你好，{personInfoResponse.Name}({personInfoResponse.StudentID})");

Console.WriteLine("正在获取日历");
var calendar = await acaAff.GetCalendar();  // 使用扩展方法发送请求
Console.WriteLine($"本周为第{calendar.CurrentWeek}周");
```

# 项目结构

## OhMyDearGpnu
就是这个Api的小Demo

## OhMyDearGpnu.Api
负责Api的实现，将自己的请求转换为学校后端的数据格式  
（没写完）

## OhMyDearGpnu.Api.Test
单元测试（还没写）

## OhMyDearGpnu.Api.SourceGenerator
用到的源生成器
