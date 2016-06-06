using BackOffice.Common;
using BackOffice.Interfaces;

namespace BackOffice
{
    internal class EventHandler
    {
        internal void Handle(IEvent upcomingEvent)
        {
            Logging.Log().Debug("Handling event {upcomingEvent}", upcomingEvent.Name);
        }
    }
}
