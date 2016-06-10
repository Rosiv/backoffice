using BackOffice.Jobs.Interfaces;
using System.Collections.Generic;

namespace BackOffice.Interfaces
{
    internal interface IRule
    {
        bool IsApplicable();

        List<IJob<IJobDto>> CreateJobs();
    }
}
