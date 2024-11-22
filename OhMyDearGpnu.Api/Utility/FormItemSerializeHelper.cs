using OhMyDearGpnu.Api.Requests;
using OhMyDearGpnu.Api.Serializer;

using System.Reflection;

using OhMyDearGpnu.Api.Cas;
using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.Utility;

public static class FormItemSerializeHelper
{
    private static readonly Dictionary<Type, BaseSerializer> serializers = new();

    static FormItemSerializeHelper()
    {
        InitCustomSerializer();
    }

    public static void InitCustomSerializer()
    {
        AddCustomSerializer(typeof(Captcha), new CaptchaSerializer());
        AddCustomSerializer(typeof(CasCaptcha), new CasCaptchaSerializer());
    }

    public static void AddCustomSerializer(Type type, BaseSerializer serializer)
    {
        serializers.Add(type, serializer);
    }

    private static BaseSerializer? GetSerializer(Type type)
    {
        return serializers.GetValueOrDefault(type);
    }

    public static string? Serialize<T>(T value)
    {
        var serializer = GetSerializer(typeof(T));
        return serializer != null ? serializer.Serialize(value) : value?.ToString();
    }
}