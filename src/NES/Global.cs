// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   Defines the Global type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;

    internal class Global
    {
        #region Static Fields

        private static IEnumerable<Type> _typesToScan = new List<Type>();

        #endregion

        #region Properties

        internal static IEnumerable<Type> TypesToScan
        {
            get
            {
                return _typesToScan;
            }

            set
            {
                _typesToScan = value;
            }
        }

        #endregion
    }
}