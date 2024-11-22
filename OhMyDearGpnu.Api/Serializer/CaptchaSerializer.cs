using System.Reflection;

using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.Serializer;

internal class CaptchaSerializer : BaseSerializer<Captcha>
{
    public override string? Serialize(Captcha? value)
    {
        return value?.value;
    }

    public override KeyValuePair<string, string>? Serialize(Captcha? value, Attribute[] attributes, FieldInfo field, string keyName)
    {
        if (value == null || value.value == null)
            return null;

        return new KeyValuePair<string, string>(keyName, value.value);
    }
}