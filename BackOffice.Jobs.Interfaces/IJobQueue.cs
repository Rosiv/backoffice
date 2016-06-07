namespace BackOffice.Jobs.Interfaces
{
    internal interface IJobQueue
    {
        void Push(IJob job);
        IJob Pull();
    }
}
