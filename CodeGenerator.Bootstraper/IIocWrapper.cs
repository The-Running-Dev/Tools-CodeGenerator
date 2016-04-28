using System;
using System.Collections.Generic;

namespace CodeGenerator.Bootstraper
{
    public interface IIocWrapper
    {
        T GetNamedInstance<T>(string name) where T : class;

        object GetNamedInstance(Type serviceType, string name);

        T GetService<T>() where T : class;

        IEnumerable<T> GetServices<T>();

        object GetService(Type serviceType);

        IEnumerable<object> GetServices(Type serviceType);

        IIocWrapper AddServices<TServiceType, TInstanceType>() where TInstanceType : TServiceType;

        IIocWrapper AddServices<T>(object instance);

        IIocWrapper SetService<T>(object instance);

        IIocWrapper AddServices(Type serviceType, object instance);

        IIocWrapper SetService(Type serviceType, object instance);
    }
}