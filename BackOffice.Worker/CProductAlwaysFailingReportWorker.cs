using BackOffice.Jobs.Interfaces;
using BackOffice.Jobs.Reports;
using System;

namespace BackOffice.Worker
{
    public class CProductAlwaysFailingReportWorker : IJobWorker
    {
        private readonly AlwaysFailingReport report;

        public CProductAlwaysFailingReportWorker(AlwaysFailingReport report)
        {
            this.report = report;
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
