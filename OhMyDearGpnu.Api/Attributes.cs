using AngleSharp.XPath;

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
    public class FromPageCacheAttribute : Attribute
    {
        public readonly string pageIdentifier;
        public readonly string? xPath;
        public readonly string? selector;

        public FromPageCacheAttribute(string pageIdentifier, string? selector = null, string? xPath = null)
        {
            this.pageIdentifier = pageIdentifier;
            this.xPath = xPath;
            this.selector = selector;
        }

        public string? ParseValue(PageCache page)
        {
            if (page.Document == null)
                throw new NullReferenceException("page's document couldn't be null");

            if (xPath != null)
            {
                var value = page.Document.Body.SelectSingleNode(xPath)?.NodeValue;
                if (value != null)
                    return value;
            }
            if (selector != null)
            {
                var value = page.Document.QuerySelector(selector)?.NodeValue;
                if (value != null)
                    return value;
            }
            return null;
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
    public class CustomSerializerAttribute : Attribute
    {
        public Type serializerType;

        public CustomSerializerAttribute(Type serializerType)
        {
            this.serializerType = serializerType;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AliasAttribute : Attribute
    {
        public string alias;

        public AliasAttribute(string alias)
        {
            this.alias = alias;
        }
    }
}
