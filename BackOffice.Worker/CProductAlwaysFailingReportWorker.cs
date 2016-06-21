using BackOffice.Jobs.Interfaces;
using BackOffice.Jobs.Reports;
using System;
using System.Threading;

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
            Thread.Sleep(10 * 1000);
            throw new NotImplementedException();
        }
    }
}
