using System.Reflection;

using OhMyDearGpnu.Api.Cas;
using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.Serializer;

internal class CasCaptchaSerializer : BaseSerializer<CasCaptcha>
{
    public override string? Serialize(CasCaptcha? value)
    {
        return value?.value;
    }

    public override KeyValuePair<string, string>? Serialize(CasCaptcha? value, Attribute[] attributes, FieldInfo field, string keyName)
    {
        if (value?.value == null)
            return null;

        return new KeyValuePair<string, string>(keyName, value.value);
    }
}