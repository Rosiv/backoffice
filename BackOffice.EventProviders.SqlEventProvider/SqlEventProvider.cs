using BackOffice.Common;
using BackOffice.Events;
using BackOffice.Interfaces;
using System;
using System.Threading;

namespace BackOffice.EventProviders.SqlEventProvider
{
    internal class SqlEventProvider : IEventProvider
    {
        public IEvent Next()
        {
            Logging.Log().Debug("Waiting for upcoming event...");

            Thread.Sleep(new Random().Next(1,10000));

            return new SimpleEvent("SQL Event");
        }
    }
}
