using BackOffice.Common;
using BackOffice.Jobs.Interfaces;
using BackOffice.Jobs.Reports;
using System;

namespace BackOffice.Worker
{
    public class JobHandler
    {
        public void Handle(IJob<IJobData> job)
        {
            Type type = Type.GetType(job.Type);

            switch (type.Name)
            {
                case nameof(SimpleTxtReport):
                    Logging.Log().Information("Handling job {job}", nameof(SimpleTxtReport));
                    new AProductSimpleTxtReportWorker((SimpleTxtReport)job).Start();
                    break;
                case (nameof(SimpleExcelReport)):
                    Logging.Log().Information("Handling job {job}", nameof(SimpleExcelReport));
                    new BProductSimpleExcelReportWorker((SimpleExcelReport)job).Start();
                    break;
            }
        }
    }
}
