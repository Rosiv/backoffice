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
    internal class BProductsReportsRule : SqlProductRuleBase
    {
        private ProductMessage message;

        public BProductsReportsRule(IEvent ev) : base(ev)
        { }

        public override List<IJob<IJobData>> CreateJobs()
        {
            var upcomingEvent = (SqlEvent)base.ev;
            var job = new SimpleExcelReport(this.message);
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
                bool result = message.Product.Name.StartsWith("B", StringComparison.InvariantCultureIgnoreCase);
                Logging.Log().Information("Product name starts with B => {result}", result);
                return result;
            }

            return false;
        }
    }
}
