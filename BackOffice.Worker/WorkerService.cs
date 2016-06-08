using BackOffice.Jobs.Queues.MongoDB;

namespace BackOffice.Worker
{
    internal class WorkerService
    {
        public void Start()
        {
            var xxx = new MongoDBJobQueue();
            xxx.Pull();
        }
    }
}
