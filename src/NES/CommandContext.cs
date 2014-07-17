// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandContext.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The command context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The command context.
    /// </summary>
    public class CommandContext
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the headers.
        /// </summary>
        public Dictionary<string, object> Headers { get; set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        #endregion
    }
}