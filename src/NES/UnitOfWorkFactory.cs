// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkFactory.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The unit of work factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The unit of work factory.
    /// </summary>
    public static class UnitOfWorkFactory
    {
        #region Static Fields

        [ThreadStatic]
        private static IUnitOfWork _current;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the current.
        /// </summary>
        public static IUnitOfWork Current
        {
            get
            {
                return _current;
            }

            internal set
            {
                _current = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The begin.
        /// </summary>
        public static void Begin()
        {
            _current = DI.Current.Resolve<IUnitOfWork>();
        }

        /// <summary>
        ///     The end.
        /// </summary>
        public static void End()
        {
            _current = null;
        }

        #endregion
    }
}