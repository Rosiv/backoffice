using BackOffice.Jobs.Interfaces;
using System.Collections.Generic;

namespace BackOffice.Interfaces
{
    public interface IRule
    {
        bool IsApplicable();

        List<IJob<IJobData>> CreateJobs();
    }
}
