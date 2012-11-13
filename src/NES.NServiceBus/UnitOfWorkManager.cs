using System;
using NServiceBus.UnitOfWork;

namespace NES.NServiceBus
{
    public class UnitOfWorkManager : IManageUnitsOfWork
    {
        public void Begin()
        {
        }

        public void End(Exception ex = null)
        {
            if (ex == null)
            {
                UnitOfWorkFactory.Current.Commit();
            }
        }
    }
}