using BackOffice.Common;
using BackOffice.Events;
using BackOffice.Interfaces;
using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BackOffice.Rules
{
    internal abstract class SqlProductRuleBase : IRule
    {
        protected IEvent ev;

        public SqlProductRuleBase(IEvent ev)
        {
            this.ev = ev;
        }

        public abstract List<IJob<IJobDto>> CreateJobs();

        public abstract bool IsApplicable();

        protected bool CheckEventType()
        {
            bool result = ev is SqlEvent;
            Logging.Log().Information("Event is SqlEvent => {result}", result);

            return result;
        }

        protected bool CheckMessageType()
        {
            var upcomingEvent = ((SqlEvent)this.ev);
            var result = string.Equals(((SqlEvent)this.ev).MessageType, "ProductChanged_Msg", StringComparison.InvariantCultureIgnoreCase);
            Logging.Log().Information("Event message type is ProductChanged_Msg => {result}", result);

            return result;
        }

        protected bool TryMapMessage(out Product product)
        {
            product = null;

            try
            {
                var upcomingEvent = ((SqlEvent)this.ev);
                XmlSerializer deserializer = new XmlSerializer(typeof(Product));
                using (TextReader reader = new StringReader(upcomingEvent.Message))
                {
                    product = (Product)deserializer.Deserialize(reader);
                    Logging.Log().Information("Message deserialized to 'Product' object.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logging.Log().Warning("Sql event deserialization error: {error}", ex);
                return false;
            }
        }
    }
}