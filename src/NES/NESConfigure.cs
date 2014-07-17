// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NESConfigure.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The nes configure.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    /// <summary>
    ///     The nes configure.
    /// </summary>
    public class NESConfigure
    {
        #region Static Fields

        private static NESConfigure configure;

        #endregion

        #region Constructors and Destructors

        private NESConfigure()
        {
            configure = new NESConfigure();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The initialize.
        /// </summary>
        /// <returns>
        ///     The <see cref="NESConfigure" />.
        /// </returns>
        public static NESConfigure Initialize()
        {
            return configure;
        }

        #endregion
    }
}