// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventSerializer.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventSerializer interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    /// <summary>
    ///     The EventSerializer interface.
    /// </summary>
    public interface IEventSerializer
    {
        #region Public Methods and Operators

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object Deserialize(string data);

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="event">
        /// The event.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string Serialize(object @event);

        #endregion
    }
}