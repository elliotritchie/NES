// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerFactory.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The logger factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The logger factory.
    /// </summary>
    public class LoggerFactory
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the create.
        /// </summary>
        public static Func<Type, ILogger> Create { get; set; }

        #endregion
    }
}