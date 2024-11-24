using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace OhMyDearGpnu.Api;

public class SimpleServiceContainer
{
    // Key: interfaceType, Value: implementationInstance
    private readonly Dictionary<Type, object> livingServices;

    // Key: typeof(TService), Value: (typeof(ImplementationInstance) ,Func<TImplementation>)
    private readonly Dictionary<Type, (Type implementationType, Func<SimpleServiceContainer, object> factory)> lazyingServices;

    public SimpleServiceContainer()
    {
        livingServices = new Dictionary<Type, object>(16);
        lazyingServices = new Dictionary<Type, (Type, Func<SimpleServiceContainer, object>)>(16);
    }

    public object Locate(Type serviceType)
    {
        if (TryLocate(serviceType, out var service))
            return service;

        throw new InvalidOperationException($"Unable to locate service for serviceType '{serviceType.FullName}'");
    }

    public bool TryLocate(Type serviceType, [NotNullWhen(true)] out object? service)
    {
        if (livingServices.TryGetValue(serviceType, out service))
            return true;

        // 新建一个
        if (lazyingServices.TryGetValue(serviceType, out var value))
        {
            var implementation = CreateService(value.implementationType, value.factory);
            var enumerator = lazyingServices.GetEnumerator();
            MoveSameImplementationToLivingList(ref enumerator, value.implementationType, implementation);
            livingServices.TryAdd(value.implementationType, implementation);

            service = implementation;
            return true;
        }

        service = null;
        return false;
    }

    public object CreateService(Type implementationType, Func<SimpleServiceContainer, object> factory)
    {
        var implementation = factory.Invoke(this);
        switch (implementation)
        {
            case null:
                throw new NoNullAllowedException("The implementation instance can't be null");
            case Task task:
                task.GetAwaiter().GetResult();
                implementation = implementation.GetType().GetProperty("Result")!.GetValue(implementation);
                break;
        }

        if (implementation == null)
            throw new NoNullAllowedException("The implementation instance can't be null");
        var realImplType = implementation.GetType();
        if (implementationType != realImplType)
            throw new InvalidOperationException($"A unexpected implementation type {realImplType.FullName} was build, but expected is {implementationType}");
        return implementation;
    }

    private void MoveSameImplementationToLivingList(ref Dictionary<Type, (Type implementationType, Func<SimpleServiceContainer, object> factory)>.Enumerator enumerator, Type implementationType, object implementation)
    {
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Value.implementationType != implementationType)
                continue;
            var serviceType = enumerator.Current.Key;
            MoveSameImplementationToLivingList(ref enumerator, implementationType, implementation);
            livingServices.Add(serviceType, implementation);
            return;
        }
    }

    public void Register(Type interfaceType, Type implementationType, Func<SimpleServiceContainer, object> factory)
    {
        lazyingServices[interfaceType] = (implementationType, factory);

        foreach (var kvp in livingServices)
        {
            // 已经实例化哩
            if (kvp.Key == interfaceType)
                return;

            if (kvp.Value.GetType() == implementationType)
            {
                livingServices.Add(interfaceType, kvp.Value);
                return;
            }
        }
    }

    public void AddExisted(Type interfaceType, object implementation)
    {
        if (livingServices.ContainsKey(interfaceType))
            return;

        lazyingServices.Remove(interfaceType);
        livingServices.Add(interfaceType, implementation);
    }
}