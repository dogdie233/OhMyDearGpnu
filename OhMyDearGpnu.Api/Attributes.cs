using AngleSharp.XPath;

namespace OhMyDearGpnu.Api;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class AliasAttribute(string alias) : Attribute
{
    public string alias = alias;
}