namespace OhMyDearGpnu.Api
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FormItemAttribute : Attribute
    {
        public readonly string name;

        public FormItemAttribute(string name)
        {
            this.name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class FromPageHiddenAttribute : Attribute
    {
        public readonly string pageIdentifier;
        public readonly string id;

        public FromPageHiddenAttribute(string pageIdentifier, string id)
        {
            this.pageIdentifier = pageIdentifier;
            this.id = id;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EncryptAttribute : Attribute
    {
        public readonly string publicKeyUri;

        public EncryptAttribute(string publicKeyUri)
        {
            this.publicKeyUri = publicKeyUri;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CaptchaSerializationBehaviourAttribute : Attribute
    {
        public enum Type
        {
            Value,
            Timestamp
        }

        public readonly Type type;

        public CaptchaSerializationBehaviourAttribute(Type type)
        {
            this.type = type;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CustomSerializerAttribute : Attribute
    {
        public Type serializerType;

        public CustomSerializerAttribute(Type serializerType)
        {
            this.serializerType = serializerType;
        }
    }
}
