using BackOffice.Common;
using BackOffice.Events;
using BackOffice.Interfaces;

namespace BackOffice
{
    internal class EventHandler
    {
        internal void Handle(IEvent upcomingEvent)
        {
            if (upcomingEvent is SqlEvent)
            {
                SqlEvent e = (SqlEvent)upcomingEvent;
                Logging.Log().Debug("Handling SQL event. Message type: {messageType} message: {message}", e.MessageType, e.Message);
            }
            Logging.Log().Debug("Handling event {upcomingEvent}", upcomingEvent.Name);
        }
    }
}
