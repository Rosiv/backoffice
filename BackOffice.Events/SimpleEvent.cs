using BackOffice.Interfaces;

namespace BackOffice.Events
{
    public class SimpleEvent : IEvent
    {
        private readonly string name;

        public SimpleEvent(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}
