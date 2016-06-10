namespace BackOffice.Jobs.Interfaces
{
    internal interface IJob<out T> where T : IJobDto
    {
        string Type { get; }

        string Name { get; }

        T Dto { get; }
    }
}
