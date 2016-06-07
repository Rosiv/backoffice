namespace BackOffice.Events
{
    internal class SqlEvent : SimpleEvent
    {
        public SqlEvent(string name, string messageType, string message) : base(name)
        {
            this.MessageType = messageType;
            this.Message = message;
        }

        public string MessageType { get; private set; }
        public string Message { get; private set; }
    }
}
