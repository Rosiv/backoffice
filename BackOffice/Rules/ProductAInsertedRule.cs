using System;
using System.Collections.Generic;
using BackOffice.Interfaces;
using BackOffice.Jobs.Interfaces;
using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Reports;

namespace BackOffice.Rules
{
    internal class ProductAInsertedRule : SqlProductRuleBase
    {
        private Product product;

        public ProductAInsertedRule(IEvent ev) : base(ev)
        { }

        public override List<IJob<IJobDto>> CreateJobs()
        {
            var job = new AProductSimpleTxtReport(this.product);
            var list = new List<IJob<IJobDto>> { job };

            return list;
        }

        public override bool IsApplicable()
        {
            if (
                base.CheckEventType() &&
                base.CheckMessageType("Inserted") &&
                base.TryMapMessage(out this.product))
            {
                return this.product.Name.StartsWith("A", StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
    }
}
