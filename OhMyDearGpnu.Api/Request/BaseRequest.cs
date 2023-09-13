using System.Reflection;

namespace OhMyDearGpnu.Api.Request
{
    public abstract class BaseRequest
    {
        public abstract string Uri { get; }

        public async Task FillAutoFieldAsync(SimpleServiceContainer serviceContainer)
        {

        }

        public IEnumerable<KeyValuePair<string, string>> GetFormItems(SimpleServiceContainer serviceContainer)
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
                var itemName = ((FormItemAttribute)info.attributes.First(attr => attr.GetType() == typeof(FormItemAttribute))).name;

                var item = RequestSerializationHelper.ResolveField(info.field, this, info.attributes, itemName);
                if (item != null)
                    result.Add(item.Value);
            }
            PostProcessFormData(result);
            return result;
        }

        protected virtual void PostProcessFormData(List<KeyValuePair<string, string>> data)
        {
        }
    }
}
