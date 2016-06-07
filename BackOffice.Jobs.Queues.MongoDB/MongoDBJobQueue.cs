using System;
using BackOffice.Jobs.Interfaces;

namespace BackOffice.Jobs.Queues.MongoDB
{
    internal class MongoDBJobQueue : IJobQueue
    {
        public IJob Pull()
        {
            throw new NotImplementedException();
        }

        public void Push(IJob job)
        {
            throw new NotImplementedException();
        }
    }
}
