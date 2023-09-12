namespace OhMyDearGpnu.Api
{
    public class SimpleServiceContainer
    {
        // Key: interfaceType, Value: implementationInstance
        private readonly Dictionary<Type, object> _livingServices;
        // Key: typeof(TService), Value: (typeof(ImplementationInstance) ,Func<TImplementation>)
        private readonly Dictionary<Type, (Type implementationType, Func<SimpleServiceContainer, object> factory)> _lazyingServices;

        public SimpleServiceContainer()
        {
            _livingServices = new Dictionary<Type, object>(16);
            _lazyingServices = new Dictionary<Type, (Type, Func<SimpleServiceContainer, object>)>(16);
        }

        public object Locate(Type serviceType)
        {
            if (_livingServices.TryGetValue(serviceType, out var service))
                return service;

            // 新建一个
            if (_lazyingServices.TryGetValue(serviceType, out var value))
            {
                var implementation = value.factory.Invoke(this);
                var realImplType = implementation.GetType();
                if (realImplType != value.implementationType)
                {
                    throw new InvalidOperationException($"A unexpected implementation type {realImplType.FullName} was build, but expected is {value.implementationType}");
                }

                var enumerator = _lazyingServices.GetEnumerator();
                MoveSameImplementationToLivingList(ref enumerator, realImplType, implementation);
                if (!_livingServices.ContainsKey(realImplType))
                    _livingServices.Add(realImplType, implementation);

                return implementation;
            }

            throw new InvalidOperationException($"Unable to locate service for serviceType '{serviceType.FullName}'");
        }

        private void MoveSameImplementationToLivingList(ref Dictionary<Type, (Type implementationType, Func<SimpleServiceContainer, object> factory)>.Enumerator enumerator, Type implementationType, object implementation)
        {
            Type serviceType;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.implementationType != implementationType)
                    continue;
                serviceType = enumerator.Current.Key;
                MoveSameImplementationToLivingList(ref enumerator, implementationType, implementation);
                _livingServices.Add(serviceType, implementation);
                return;
            }
        }

        public void Register(Type interfaceType, Type implementationType, Func<SimpleServiceContainer, object> factory)
        {
            _lazyingServices[interfaceType] = (implementationType, factory);

            foreach (var kvp in _livingServices)
            {
                // 已经实例化哩
                if (kvp.Key == interfaceType)
                    return;

                if (kvp.Value.GetType() == implementationType)
                {
                    _livingServices.Add(interfaceType, kvp.Value);
                    return;
                }
            }
        }

        public void AddExisted(Type interfaceType, object implementation)
        {
            if (_livingServices.ContainsKey(interfaceType))
                return;

            _lazyingServices.Remove(interfaceType);
            _livingServices.Add(interfaceType, implementation);
        }
    }
}
