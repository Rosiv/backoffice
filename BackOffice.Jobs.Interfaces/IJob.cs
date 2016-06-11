namespace BackOffice.Jobs.Interfaces
{
    public interface IJob<out T> where T : IJobData
    {
        string Type { get; }

        string Name { get; }

        T Data { get; }
    }
}
