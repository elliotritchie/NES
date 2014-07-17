// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventSourceFactory.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventSourceFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    /// <summary>
    ///     The EventSourceFactory interface.
    /// </summary>
    public interface IEventSourceFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The create.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the event source
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        T Create<T>() where T : IEventSource;

        #endregion
    }
}