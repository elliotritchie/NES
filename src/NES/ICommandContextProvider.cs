// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandContextProvider.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The CommandContextProvider interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    /// <summary>
    ///     The CommandContextProvider interface.
    /// </summary>
    public interface ICommandContextProvider
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The get.
        /// </summary>
        /// <returns>
        ///     The <see cref="CommandContext" />.
        /// </returns>
        CommandContext Get();

        #endregion
    }
}