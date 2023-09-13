using System.Reflection;

namespace OhMyDearGpnu.Api
{
    internal class Deserializer<T> where T : class
    {
        private Dictionary<string, List<PropertyInfo>> infos;
        private T target;

        public Deserializer(T target)
        {
            infos = new Dictionary<string, List<PropertyInfo>>();
            IEnumerable<(PropertyInfo info, AliasAttribute attr)> properties =
                target.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(p => (p, p.GetCustomAttribute<AliasAttribute>()))
                .Where(info => info.Item2 != null && info.p.CanWrite)!;

            this.target = target;

            foreach (var property in properties)
            {
                if (!infos.TryGetValue(property.attr.alias, out var list))
                {
                    list = new List<PropertyInfo>();
                    infos[property.attr.alias] = list;
                }
                list.Add(property.info);
            }
        }

        public void Write<TValue>(string alias, TValue value)
        {
            if (!infos.TryGetValue(alias, out var list))
                return;

            foreach (var property in list)
            {
                if (value == null)
                {
                    property.SetValue(target, null);
                    continue;
                }

                if (property.PropertyType == value.GetType())
                    property.SetValue(target, value);
            }
        }

        public static Deserializer<T> Create(T target)
            => new Deserializer<T>(target);
    }
}
