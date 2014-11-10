using System;
using System.Collections.Generic;
using NES.Contracts;

namespace NES
{
    /// <summary>
    /// Based on the DependencyInjectionContainer implementation in the Telerik MVC extensions
    /// http://www.telerik.com/products/aspnet-mvc.aspx
    /// </summary>
    public class DependencyInjectionContainer : IDependencyInjectionContainer
    {
        private readonly IDictionary<Type, object> _factories = new Dictionary<Type, object>();

        public TService Resolve<TService>()
        {
            object factory;

            if (_factories.TryGetValue(typeof(TService), out factory))
            {
                return ((Func<TService>)factory)();
            }

            return default(TService);
        }

        public void Register<TService>(TService service)
        {
            _factories[typeof(TService)] = service;
        }

        public void Register<TService>(Func<TService> factory)
        {
            _factories[typeof(TService)] = factory;
        }

        public void Register<TService>(Func<IDependencyInjectionContainer, TService> factory)
        {
            Func<TService> partialFactory = () => factory(this);

            _factories[typeof(TService)] = partialFactory;
        }

        public void Register<TService, TArg>(Func<TArg, TService> factory)
        {
            Func<TService> partialFactory = () => factory(Resolve<TArg>());

            _factories[typeof(TService)] = partialFactory;
        }

        public void Register<TService, TArg1, TArg2>(Func<TArg1, TArg2, TService> factory)
        {
            Func<TService> partialFactory = () => factory(Resolve<TArg1>(), Resolve<TArg2>());

            _factories[typeof(TService)] = partialFactory;
        }

        public void Register<TService, TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, TService> factory)
        {
            Func<TService> partialFactory = () => factory(Resolve<TArg1>(), Resolve<TArg2>(), Resolve<TArg3>());

            _factories[typeof(TService)] = partialFactory;
        }

        public void Register<TService, TArg1, TArg2, TArg3, TArg4>(Func<TArg1, TArg2, TArg3, TArg4, TService> factory)
        {
            Func<TService> partialFactory = () => factory(Resolve<TArg1>(), Resolve<TArg2>(), Resolve<TArg3>(), Resolve<TArg4>());

            _factories[typeof(TService)] = partialFactory;
        }
    }
}