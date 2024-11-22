using System.Reflection;

namespace OhMyDearGpnu.Api.Serializer;

public abstract class BaseSerializer
{
    public readonly Type targetType;

    public abstract string? Serialize(object? value);
    public abstract KeyValuePair<string, string>? Serialize(object? value, Attribute[] attributes, FieldInfo field, string keyName);

    public BaseSerializer(Type targetType)
    {
        this.targetType = targetType;
    }
}

public abstract class BaseSerializer<T> : BaseSerializer
{
    public BaseSerializer() : base(typeof(T))
    {
    }

    public override string? Serialize(object? value)
    {
        return Serialize((T?)value);
    }

    public abstract string? Serialize(T? value);

    public override KeyValuePair<string, string>? Serialize(object? value, Attribute[] attributes, FieldInfo field, string keyName)
    {
        return Serialize((T?)value, attributes, field, keyName);
    }

    public abstract KeyValuePair<string, string>? Serialize(T? value, Attribute[] attributes, FieldInfo field, string keyName);
}