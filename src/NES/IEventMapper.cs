// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventMapper.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventMapper interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The EventMapper interface.
    /// </summary>
    public interface IEventMapper
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get mapped type for.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        Type GetMappedTypeFor(Type type);

        #endregion
    }
}