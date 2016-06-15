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
    internal class CProductsAlwaysFailingRule : SqlProductRuleBase
    {
        private ProductMessage message;

        public CProductsAlwaysFailingRule(IEvent ev) : base(ev)
        { }

        public override List<IJob<IJobData>> CreateJobs()
        {
            var upcomingEvent = (SqlEvent)base.ev;
            var job = new AlwaysFailingReport(this.message);
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
                bool result = message.Product.Name.StartsWith("C", StringComparison.InvariantCultureIgnoreCase);
                Logging.Log().Information("Product name starts with C => {result}", result);
                return result;
            }

            return false;
        }
    }
}
