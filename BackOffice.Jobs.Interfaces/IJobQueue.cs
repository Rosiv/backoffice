namespace BackOffice.Jobs.Interfaces
{
    internal interface IJobQueue
    {
        void Push(IJob<IJobDto> job);
        IJob<IJobDto> Pull();
    }
}
