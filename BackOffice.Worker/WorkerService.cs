using BackOffice.Jobs.Queues.MongoDB;

namespace BackOffice.Worker
{
    public class WorkerService
    {
        public void Start()
        {
            var xxx = new MongoDBJobQueue();
            xxx.Pull();
        }
    }
}
