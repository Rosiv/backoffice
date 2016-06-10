namespace BackOffice.Jobs.Interfaces
{
    public interface IJob<out T> where T : IJobDto
    {
        string Type { get; }

        string Name { get; }

        T Dto { get; }
    }
}
