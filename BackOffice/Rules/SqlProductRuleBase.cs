using BackOffice.Events;
using BackOffice.Interfaces;
using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Interfaces;
using System.Collections.Generic;

namespace BackOffice.Rules
{
    internal abstract class SqlProductRuleBase : IRule
    {
        private IEvent ev;

        public SqlProductRuleBase(IEvent ev)
        {
            this.ev = ev;
        }

        public abstract List<IJob<IJobDto>> CreateJobs();

        public abstract bool IsApplicable();

        protected bool CheckEventType()
        {
            return ev is SqlEvent;
        }

        protected bool CheckMessageType(string messageType)
        {
            return false;
        }

        protected bool TryMapMessage(out Product product)
        {
            product = null;
            return false;
        }
    }
}