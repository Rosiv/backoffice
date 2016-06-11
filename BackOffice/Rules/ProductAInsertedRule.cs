using System;
using System.Collections.Generic;
using BackOffice.Interfaces;
using BackOffice.Jobs.Interfaces;
using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Reports;
using BackOffice.Events;

namespace BackOffice.Rules
{
    internal class ProductAInsertedRule : SqlProductRuleBase
    {
        private ProductMessage message;

        public ProductAInsertedRule(IEvent ev) : base(ev)
        { }

        public override List<IJob<IJobData>> CreateJobs()
        {
            var upcomingEvent = (SqlEvent)base.ev;
            var job = new AProductSimpleTxtReport(this.message);
            var list = new List<IJob<IJobData>> { job };

            return list;
        }

        public override bool IsApplicable()
        {
            if (
                base.CheckEventType() &&
                base.CheckMessageType() &&
                base.TryMapMessage(out this.message))
            {
                return this.message.Product.Name.StartsWith("A", StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
    }
}
