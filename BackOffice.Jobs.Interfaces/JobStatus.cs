namespace BackOffice.Jobs.Interfaces
{
    public enum JobStatus
    {
        Created = 0,
        Enqueued,
        InProgress,
        Done,
        Failed
    }
}
