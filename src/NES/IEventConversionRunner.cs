// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventConversionRunner.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventConversionRunner interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    /// <summary>
    ///     The EventConversionRunner interface.
    /// </summary>
    public interface IEventConversionRunner
    {
        #region Public Methods and Operators

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="event">
        /// The event.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object Run(object @event);

        #endregion
    }
}