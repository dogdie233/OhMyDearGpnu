# Oh My Dear Gpnu
一个 _广东技术师范大学教务管理系统_ 的后端API Wrapper  
__本项目并非广东技术师范大学官方的项目__  
> 高不高性能我就不知道了XD

# 使用方法

1. 添加OhMyDearGpnu.Api至项目引用
2. 享用接口吧

```CSharp
GpnuClient client = new GpnuClient();  // 创建一个客户端实例
Cpatcha captcha = await client.SendRequest(new GetCaptchaRequest(null)).data;  // 使用实例化请求的方式获取验证码，这里忽略是否成功
captcha.value = "这里自己解验证码";
Response loginResponse = await client.Login("学号", "密码", captcha);  // 使用扩展方法的方式获取验证码
if (!loginResponse.IsSucceeded)
{
    throw new Exception("登录失败，" + loginResponse.message);
}
Console.WriteLine("登录成功");
```

# 项目结构

## OhMyDearGpnu
就是这个Api的小Demo

## OhMyDearGpnu.Api
负责Api的实现，将自己的请求转换为学校后端的数据格式  
（没写完）

## OhMyDearGpnu.Api.Test
单元测试（还没写）
