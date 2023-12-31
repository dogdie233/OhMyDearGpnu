﻿using System.Reflection;
using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.Serializer
{
    internal class CaptchaSerializer : BaseSerializer<Captcha>
    {
        public override KeyValuePair<string, string>? Serialize(Captcha? value, Attribute[] attributes, FieldInfo field, string keyName)
        {
            if (value == null || value.value == null)
                return null;

            return new(keyName, value.value);
        }
    }
}
