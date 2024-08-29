using System.Reflection;

using OhMyDearGpnu.Api.Modules;
using OhMyDearGpnu.Api.Modules.Cas;

namespace OhMyDearGpnu.Api.Serializer;

internal class CasCaptchaSerializer : BaseSerializer<CasCaptcha>
{
    public override KeyValuePair<string, string>? Serialize(CasCaptcha? value, Attribute[] attributes, FieldInfo field, string keyName)
    {
        if (value?.value == null)
            return null;

        return new KeyValuePair<string, string>(keyName, value.value);
    }
}