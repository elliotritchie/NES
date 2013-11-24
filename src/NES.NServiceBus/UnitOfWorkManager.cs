using System;
using NServiceBus.UnitOfWork;

namespace NES.NServiceBus
{
    public class UnitOfWorkManager : IManageUnitsOfWork
    {
        public void Begin()
        {
            UnitOfWorkFactory.Begin();
        }

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
    }
}