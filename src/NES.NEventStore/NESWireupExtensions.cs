// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NESWireupExtensions.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The nes wireup extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore
{
    using global::NEventStore;

    /// <summary>
    ///     The nes wireup extensions.
    /// </summary>
    public static class NESWireupExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The nes.
        /// </summary>
        /// <param name="wireup">
        /// The wireup.
        /// </param>
        /// <returns>
        /// The <see cref="NESWireup"/>.
        /// </returns>
        public static NESWireup NES(this Wireup wireup)
        {
            return new NESWireup(wireup);
        }

        #endregion
    }
}