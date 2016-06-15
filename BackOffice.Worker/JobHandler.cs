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
                case (nameof(AlwaysFailingReport)):
                    Logging.Log().Information("Handling job {job}", nameof(AlwaysFailingReport));
                    new CProductAlwaysFailingReportWorker((AlwaysFailingReport)job).Start();
                    break;
                case (nameof(SqlReport)):
                    Logging.Log().Information("Handling job {job}", nameof(SqlReport));
                    new DProductSqlReportWorker((SqlReport)job).Start();
                    break;
            }
        }
    }
}
