namespace BackOffice.Jobs.Interfaces
{
    public interface IJobQueue
    {
        void Push(IJob<IJobDto> job);
        IJob<IJobDto> Pull();
    }
}
