using System;
using System.Collections.Generic;

using StructureMap;

namespace CodeGenerator.Bootstraper
{
    public class IocWrapper : IIocWrapper
    {
        private static readonly object SyncObject = new object();
        private static IocWrapper _instance;

        public IContainer Container;

        public IocWrapper(IContainer iocContainer)
        {
            Container = iocContainer;
        }

        private IocWrapper() { }

        public static IocWrapper Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;

                }

                lock (SyncObject)
                {
                    if (_instance != null)
                    {
                        return _instance;
                    }

                    _instance = new IocWrapper();
                }

                return _instance;
            }
            set
            {
                lock (SyncObject)
                {
                    _instance = value;
                }
            }
        }

        public T GetNamedInstance<T>(string name) where T : class
        {
            return Container.GetInstance<T>(name);
        }

        public object GetNamedInstance(Type serviceType, string name)
        {
            return Container.GetInstance(serviceType, name);
        }

        public T GetService<T>() where T : class
        {
            return Container.GetInstance<T>();
        }

        public T GetService<T>(Dictionary<string, object> parameters) where T : class
        {
            return Container.GetInstance<T>();
        }

        public IEnumerable<T> GetServices<T>()
        {
            IEnumerable<T> results = new List<T>();

            try
            {
                results = Container.GetAllInstances<T>();
            }
            catch (Exception) { }

            return results;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return Container.GetInstance(serviceType);
            }
            catch (Exception) { }

            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return Container.GetAllInstances(serviceType) as IEnumerable<object>;
            }
            catch (Exception) { }

            return null;
        }

        public IIocWrapper AddServices<TServiceType, TInstanceType>() where TInstanceType : TServiceType
        {
            try
            {
                Container.Configure(cfg => cfg.For<TServiceType>().Use<TInstanceType>());
            }
            catch (Exception) { }

            return this;
        }

        public IIocWrapper AddServices<T>(object instance)
        {
            return AddServices(typeof(T), instance);
        }

        public IIocWrapper SetService<T>(object instance)
        {
            return SetService(typeof(T), instance);
        }

        public IIocWrapper AddServices(Type serviceType, object instance)
        {
            try
            {
                Container.Configure(cfg => cfg.For(serviceType).Use(instance));
            }
            catch (Exception) { }

            return this;
        }

        public IIocWrapper SetService(Type serviceType, object instance)
        {
            try
            {
                Container.Inject(serviceType, instance);
            }
            catch (Exception) { }

            return this;
        }
    }
}