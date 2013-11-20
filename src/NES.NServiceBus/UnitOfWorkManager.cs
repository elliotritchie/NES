using System;
using NServiceBus.UnitOfWork;

namespace NES.NServiceBus
{
    public class UnitOfWorkManager : IManageUnitsOfWork
    {
        private static readonly ILogger Logger = LoggingFactory.BuildLogger(typeof(UnitOfWorkManager));

        public void Begin()
        {
            Logger.Debug("Begin");
            UnitOfWorkFactory.Begin();
        }

        public void End(Exception ex = null)
        {
            Logger.Debug("End. So perform commit if no exception");
            if (ex == null)
            {
                UnitOfWorkFactory.Current.Commit();
                Logger.Debug("Commit done");
            }
            else
            {
                Logger.Error(string.Format("Exception message {0} type {1}", ex.Message, ex.GetType().FullName));
            }
            UnitOfWorkFactory.End();
        }
    }
}