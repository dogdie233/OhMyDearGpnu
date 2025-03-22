using System.Diagnostics.CodeAnalysis;

namespace OhMyDearGpnu.Api;

public static class SimpleServiceContainerExtension
{
    public static SimpleServiceContainer Register<TImplementation>(this SimpleServiceContainer container)
        where TImplementation : class, new()
    {
        Register<TImplementation, TImplementation>(container);
        return container;
    }
    
    public static SimpleServiceContainer Register<TImplementation>(this SimpleServiceContainer container, Func<SimpleServiceContainer, TImplementation> serviceFactory)
        where TImplementation : class
    {
        Register<TImplementation, TImplementation>(container, serviceFactory);
        return container;
    }

    public static SimpleServiceContainer RegisterAsync<TImplementation>(this SimpleServiceContainer container, Func<SimpleServiceContainer, Task<TImplementation>> serviceFactory)
        where TImplementation : class
    {
        RegisterAsync<TImplementation, TImplementation>(container, serviceFactory);
        return container;
    }

    public static SimpleServiceContainer Register<TService, TImplementation>(this SimpleServiceContainer container)
        where TService : class
        where TImplementation : class, TService, new()
    {
        container.Register(typeof(TService), typeof(TImplementation), BuildMethod);
        return container;
        object BuildMethod(SimpleServiceContainer _) => new TImplementation();
    }

    public static SimpleServiceContainer Register<TService, TImplementation>(this SimpleServiceContainer container, Func<SimpleServiceContainer, TImplementation> serviceFactory)
        where TService : class
        where TImplementation : class, TService
    {
        container.Register(typeof(TService), typeof(TImplementation), serviceFactory);
        return container;
    }

    public static SimpleServiceContainer RegisterAsync<TService, TImplementation>(this SimpleServiceContainer container, Func<SimpleServiceContainer, Task<TImplementation>> serviceFactory)
        where TService : class
        where TImplementation : class, TService
    {
        container.Register(typeof(TService), typeof(TImplementation), BuildMethod);
        return container;
        object BuildMethod(SimpleServiceContainer c) => serviceFactory(c).GetAwaiter().GetResult();
    }

    public static T Locate<T>(this SimpleServiceContainer container)
    {
        return (T)container.Locate(typeof(T));
    }

    public static bool TryLocate<T>(this SimpleServiceContainer container, [NotNullWhen(true)] out T? service)
    {
        if (container.TryLocate(typeof(T), out var serv))
        {
            service = (T)serv;
            return true;
        }

        service = default;
        return false;
    }

    public static SimpleServiceContainer AddExisted<TService, TImplementation>(this SimpleServiceContainer container, TImplementation implementation)
        where TService : class
        where TImplementation : TService
    {
        container.AddExisted(typeof(TService), implementation);
        return container;
    }

    public static SimpleServiceContainer AddExisted<TImplementation>(this SimpleServiceContainer container, TImplementation implementation)
        where TImplementation : class
    {
        container.AddExisted(typeof(TImplementation), implementation);
        return container;
    }
}