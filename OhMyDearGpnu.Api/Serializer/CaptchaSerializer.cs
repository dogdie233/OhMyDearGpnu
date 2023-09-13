using System.Reflection;

namespace OhMyDearGpnu.Api.Serializer
{
    internal class CaptchaSerializer : BaseSerializer<Captcha>
    {
        public override KeyValuePair<string, string>? Serialize(Captcha? value, Attribute[] attributes, FieldInfo field, string keyName)
        {
            if (value == null)
                return null;

            var attr = attributes.FirstOrDefault(attr => attr.GetType() == typeof(CaptchaSerializationBehaviourAttribute));
            var type = (attr as CaptchaSerializationBehaviourAttribute)?.type ?? CaptchaSerializationBehaviourAttribute.Type.Value;
            if (type == CaptchaSerializationBehaviourAttribute.Type.Value && value.value == null)
                throw new NullReferenceException($"Capture value couldn't be null");
            return new(keyName, type switch
            {
                CaptchaSerializationBehaviourAttribute.Type.Value => value.value!,
                CaptchaSerializationBehaviourAttribute.Type.Timestamp => value.timestamp,
                _ => throw new NotSupportedException()
            });
        }
    }
}
