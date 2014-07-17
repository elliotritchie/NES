// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyInjectionContainer.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   Based on the DependencyInjectionContainer implementation in the Telerik MVC extensions
//   http://www.telerik.com/products/aspnet-mvc.aspx
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     Based on the DependencyInjectionContainer implementation in the Telerik MVC extensions
    ///     http://www.telerik.com/products/aspnet-mvc.aspx
    /// </summary>
    public class DependencyInjectionContainer : IDependencyInjectionContainer
    {
        #region Fields

        private readonly IDictionary<Type, object> _factories = new Dictionary<Type, object>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <typeparam name="TService">
        /// The type of the service
        /// </typeparam>
        public void Register<TService>(TService service)
        {
            this._factories[typeof(TService)] = service;
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of the service
        /// </typeparam>
        public void Register<TService>(Func<TService> factory)
        {
            this._factories[typeof(TService)] = factory;
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of the service
        /// </typeparam>
        public void Register<TService>(Func<IDependencyInjectionContainer, TService> factory)
        {
            Func<TService> partialFactory = () => factory(this);

            this._factories[typeof(TService)] = partialFactory;
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of the service
        /// </typeparam>
        /// <typeparam name="TArg">
        /// Argument for the factory
        /// </typeparam>
        public void Register<TService, TArg>(Func<TArg, TService> factory)
        {
            Func<TService> partialFactory = () => factory(this.Resolve<TArg>());

            this._factories[typeof(TService)] = partialFactory;
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of the service
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// Argument1 for the factory
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// Argument2 for the factory
        /// </typeparam>
        public void Register<TService, TArg1, TArg2>(Func<TArg1, TArg2, TService> factory)
        {
            Func<TService> partialFactory = () => factory(this.Resolve<TArg1>(), this.Resolve<TArg2>());

            this._factories[typeof(TService)] = partialFactory;
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// Type of the service
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// Argument1 for the factory
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// Argument2 for the factory
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// Argument3 for the factory
        /// </typeparam>
        public void Register<TService, TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, TService> factory)
        {
            Func<TService> partialFactory = () => factory(this.Resolve<TArg1>(), this.Resolve<TArg2>(), this.Resolve<TArg3>());

            this._factories[typeof(TService)] = partialFactory;
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// Type of the service
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// Argument1 for the factory
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// Argument2 for the factory
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// Argument3 for the factory
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// Argument4 for the factory
        /// </typeparam>
        public void Register<TService, TArg1, TArg2, TArg3, TArg4>(Func<TArg1, TArg2, TArg3, TArg4, TService> factory)
        {
            Func<TService> partialFactory =
                () => factory(this.Resolve<TArg1>(), this.Resolve<TArg2>(), this.Resolve<TArg3>(), this.Resolve<TArg4>());

            this._factories[typeof(TService)] = partialFactory;
        }

        /// <summary>
        ///     The resolve.
        /// </summary>
        /// <typeparam name="TService">
        ///     Type of the service
        /// </typeparam>
        /// <returns>
        ///     The <see cref="TService" />.
        /// </returns>
        public TService Resolve<TService>()
        {
            object factory;

            if (this._factories.TryGetValue(typeof(TService), out factory))
            {
                return ((Func<TService>)factory)();
            }

            return default(TService);
        }

        #endregion
    }
}