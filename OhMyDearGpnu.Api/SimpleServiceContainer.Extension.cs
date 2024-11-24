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

    public static SimpleServiceContainer Register<TImplementation>(this SimpleServiceContainer container, Func<SimpleServiceContainer, Task<TImplementation>> serviceFactory)
        where TImplementation : class
    {
        Register<TImplementation, TImplementation>(container, serviceFactory);
        return container;
    }

    public static SimpleServiceContainer Register<TService, TImplementation>(this SimpleServiceContainer container)
        where TService : class
        where TImplementation : class, TService, new()
    {
        container.Register(typeof(TService), typeof(TImplementation), BuildFactoryMethod<TImplementation>());
        return container;
    }

    public static SimpleServiceContainer Register<TService, TImplementation>(this SimpleServiceContainer container, Func<SimpleServiceContainer, TImplementation> serviceFactory)
        where TService : class
        where TImplementation : TService
    {
        container.Register(typeof(TService), typeof(TImplementation), c => serviceFactory(c));
        return container;
    }

    public static SimpleServiceContainer Register<TService, TImplementation>(this SimpleServiceContainer container, Func<SimpleServiceContainer, Task<TImplementation>> serviceFactory)
        where TService : class
        where TImplementation : TService
    {
        container.Register(typeof(TService), typeof(TImplementation), serviceFactory);
        return container;
    }

    public static T Locate<T>(this SimpleServiceContainer container)
    {
        return (T)container.Locate(typeof(T));
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

    private static Func<SimpleServiceContainer, object> BuildFactoryMethod<T>() where T : class, new()
    {
        Func<SimpleServiceContainer, object> buildMethod = _ => new T();
        return buildMethod;
    }
}