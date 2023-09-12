﻿using System.Reflection;

namespace OhMyDearGpnu.Api.Serializer
{
    public abstract class BaseSerializer
    {
        public readonly Type targetType;

        public abstract KeyValuePair<string, string>? Serialize(object? value, Attribute[] attributes, FieldInfo field, string keyName);

        public BaseSerializer(Type targetType)
        {
            this.targetType = targetType;
        }
    }

    public abstract class BaseSerializer<T> : BaseSerializer
    {
        public BaseSerializer() : base(typeof(T)) { }

        public override KeyValuePair<string, string>? Serialize(object? value, Attribute[] attributes, FieldInfo field, string keyName)
            => Serialize((T?)value, attributes, field, keyName);

        public abstract KeyValuePair<string, string>? Serialize(T? value, Attribute[] attributes, FieldInfo field, string keyName);
    }
}
