// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventSource.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventSource interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The EventSource interface.
    /// </summary>
    public interface IEventSource
    {
        #region Public Properties

        /// <summary>
        ///     Gets the bucket id.
        /// </summary>
        string BucketId { get; }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     Gets the version.
        /// </summary>
        int Version { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The flush.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        IEnumerable<object> Flush();

        /// <summary>
        /// The hydrate.
        /// </summary>
        /// <param name="events">
        /// The events.
        /// </param>
        void Hydrate(IEnumerable<object> events);

        /// <summary>
        /// The restore snapshot.
        /// </summary>
        /// <param name="memento">
        /// The memento.
        /// </param>
        void RestoreSnapshot(IMemento memento);

        /// <summary>
        ///     The take snapshot.
        /// </summary>
        /// <returns>
        ///     The <see cref="IMemento" />.
        /// </returns>
        IMemento TakeSnapshot();

        #endregion
    }
}