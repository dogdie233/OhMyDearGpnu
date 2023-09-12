using System.Reflection;

namespace OhMyDearGpnu.Api.Request
{
    public abstract class BaseRequest
    {
        public abstract string Uri { get; }

        public virtual IEnumerable<KeyValuePair<string, string>> GetFormItems(PageCacheManager? cachePageManager)
        {
            var infos = GetType().GetFields()
                .Select(field => (field: field, attributes: field.GetCustomAttributes().ToArray()))
                .Where(info => info.attributes.Any(a => a.GetType() == typeof(FormItemAttribute)))
                .ToArray();

            var result = new List<KeyValuePair<string, string>>(infos.Length);
            foreach (var info in infos)
            {
                var valueObj = info.field.GetValue(this);
                if (valueObj == null)
                    continue;

                var value = valueObj.ToString();
                var itemName = info.field.Name;
                foreach (var attribute in info.attributes)
                {
                    switch (attribute)
                    {
                        case FormItemAttribute formItem:
                            itemName = formItem.name;
                            break;
                        case FromPageHiddenAttribute fromPageHidden:
                            if (cachePageManager == null)
                                continue;


                            break;
                    }
                }
            }

            return result;
        }
    }
}
