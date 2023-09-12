using OhMyDearGpnu.Api.Request;
using OhMyDearGpnu.Api.Serializer;

using System.Reflection;

namespace OhMyDearGpnu.Api
{
    public static class RequestSerializationHelper
    {
        private static readonly List<BaseSerializer> serializers = new();

        static RequestSerializationHelper()
        {
            AddCustomSerializer(new CaptchaSerializer());
        }

        public static void AddCustomSerializer(BaseSerializer serializer)
        {
            var type = serializer.targetType;
            foreach (var ser in serializers)
            {
                if (ser.targetType == type)
                    return;
            }
            serializers.Add(serializer);
        }

        public static KeyValuePair<string, string>? ResolveField(FieldInfo field, BaseRequest request, Attribute[] attributes, string keyName)
        {
            var value = field.GetValue(request);

            // 定义在字段上的
            var fieldCustomSerializerAttr = attributes.FirstOrDefault(attr => attr.GetType() == typeof(CustomSerializerAttribute)) as CustomSerializerAttribute;
            if (fieldCustomSerializerAttr != null)
            {
                var serializer = GetSerializer(fieldCustomSerializerAttr.serializerType);
                if (serializer != null)
                    return serializer.Serialize(value, attributes, field, keyName);
            }

            // 定义在本类的
            var customSerializer = serializers.FirstOrDefault(serializer => serializer.targetType == field.FieldType);
            if (customSerializer != null)
                return customSerializer.Serialize(field.GetValue(value), attributes, field, keyName);

            // 直接ToString()
            if (value == null)
                return null;

            var str = value.ToString();
            if (str == null)
                return null;

            return new(keyName, str);
        }

        private static BaseSerializer? GetSerializer(Type serializerType)
        {
            foreach (var serializer in serializers)
            {
                if (serializer.GetType() == serializerType)
                    return serializer;
            }

            // 尝试构造一个新的
            var ctor = serializerType.GetConstructor(BindingFlags.Public, Array.Empty<Type>());
            if (ctor == null)
                return null;

            var newSerializer = (BaseSerializer)ctor.Invoke(Array.Empty<object>());
            return newSerializer;
        }
    }
}
