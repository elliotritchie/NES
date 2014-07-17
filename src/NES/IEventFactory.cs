// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventFactory.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The EventFactory interface.
    /// </summary>
    public interface IEventFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// The type of the event
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Create<T>(Action<T> action);

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object Create(Type type);

        #endregion
    }
}