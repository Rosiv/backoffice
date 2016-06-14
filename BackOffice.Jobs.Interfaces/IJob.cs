namespace BackOffice.Jobs.Interfaces
{
    public interface IJob<out T> where T : IJobData
    {
        string Id { get; set; }

        string Type { get; }

        string Name { get; }

        JobStatus Status { get; set; }

        T Data { get; }
    }
}
