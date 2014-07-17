// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDependencyInjectionContainer.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The DependencyInjectionContainer interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The DependencyInjectionContainer interface.
    /// </summary>
    public interface IDependencyInjectionContainer
    {
        #region Public Methods and Operators

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of service
        /// </typeparam>
        /// <typeparam name="TArg">
        /// The Argument
        /// </typeparam>
        void Register<TService, TArg>(Func<TArg, TService> factory);

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of service
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// The Argument1
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// The Argument2
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// The Argument3
        /// </typeparam>
        /// <typeparam name="TArg4">
        /// The Argument4
        /// </typeparam>
        void Register<TService, TArg1, TArg2, TArg3, TArg4>(Func<TArg1, TArg2, TArg3, TArg4, TService> factory);

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of service
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// The Argument1
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// The Argument2
        /// </typeparam>
        /// <typeparam name="TArg3">
        /// The Argument3
        /// </typeparam>
        void Register<TService, TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, TService> factory);

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of service
        /// </typeparam>
        /// <typeparam name="TArg1">
        /// The Argument1
        /// </typeparam>
        /// <typeparam name="TArg2">
        /// The Argument2
        /// </typeparam>
        void Register<TService, TArg1, TArg2>(Func<TArg1, TArg2, TService> factory);

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of the service
        /// </typeparam>
        void Register<TService>(Func<TService> factory);

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <typeparam name="TService">
        /// The type of the service
        /// </typeparam>
        void Register<TService>(Func<IDependencyInjectionContainer, TService> factory);

        /// <summary>
        ///     The resolve.
        /// </summary>
        /// <typeparam name="TService">
        /// The type of the service
        /// </typeparam>
        /// <returns>
        ///     The <see cref="TService" />.
        /// </returns>
        TService Resolve<TService>();

        #endregion
    }
}