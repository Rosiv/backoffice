using System;
using System.Collections.Generic;
using BackOffice.Interfaces;
using BackOffice.Jobs.Interfaces;
using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Reports;
using BackOffice.Events;
using BackOffice.Common;

namespace BackOffice.Rules
{
    internal class DProductsReportsRule : SqlProductRuleBase
    {
        private ProductMessage message;

        public DProductsReportsRule(IEvent ev) : base(ev)
        { }

        public override List<IJob<IJobData>> CreateJobs()
        {
            var upcomingEvent = (SqlEvent)base.ev;
            var job = new SqlReport(this.message);
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
                bool result = message.Product.Name.StartsWith("D", StringComparison.InvariantCultureIgnoreCase);
                Logging.Log().Information("Product name starts with D => {result}", result);
                return result;
            }

            return false;
        }
    }
}
