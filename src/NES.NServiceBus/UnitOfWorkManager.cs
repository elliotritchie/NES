// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkManager.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The unit of work manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NServiceBus
{
    using System;

    using global::NServiceBus.UnitOfWork;

    /// <summary>
    ///     The unit of work manager.
    /// </summary>
    public class UnitOfWorkManager : IManageUnitsOfWork
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The begin.
        /// </summary>
        public void Begin()
        {
            UnitOfWorkFactory.Begin();
        }

        /// <summary>
        /// The end.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        public void End(Exception ex = null)
        {
            try
            {
                if (ex == null)
                {
                    UnitOfWorkFactory.Current.Commit();
                }
            }
            finally
            {
                UnitOfWorkFactory.End();
            }
        }

        #endregion
    }
}