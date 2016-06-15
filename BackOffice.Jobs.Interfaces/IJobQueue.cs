using BackOffice.Jobs.Interfaces;

namespace BackOffice.Jobs.Interfaces
{
    public interface IJobQueue
    {
        void Push(IJob<IJobData> job);

        IJob<IJobData> Pull();

        long Count { get; }

        void SetJobStatus(IJob<IJobData> job, JobStatus status, string additionalInfo = null);
    }
}
