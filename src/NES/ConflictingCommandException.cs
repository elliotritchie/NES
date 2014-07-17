// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConflictingCommandException.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The conflicting command exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The conflicting command exception.
    /// </summary>
    [Serializable]
    public class ConflictingCommandException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictingCommandException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public ConflictingCommandException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}