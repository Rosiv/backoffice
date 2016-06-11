namespace BackOffice.Jobs.Interfaces
{
    public interface IJobQueue
    {
        void Push(IJob<IJobData> job);

        IJob<IJobData> Pull();

        long Count { get; }
    }
}
