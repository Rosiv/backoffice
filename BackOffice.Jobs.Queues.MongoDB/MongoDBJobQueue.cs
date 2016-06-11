using System;
using BackOffice.Jobs.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BackOffice.Jobs.Queues.MongoDB
{
    public class MongoDBJobQueue : IJobQueue
    {
        private const string ConnectionString = "mongodb://localhost:27017/";
        private const string DatabaseName = "backoffice";
        private const string CollectionName = "job_queue";
        private readonly MongoClient client;

        public MongoDBJobQueue()
        {
            this.client = new MongoClient(ConnectionString);
        }

        public IJob<IJobData> Pull()
        {
            var db = this.client.GetDatabase(DatabaseName);
            var collection = db.GetCollection<BsonDocument>(CollectionName);

            var document =  collection.Find(new BsonDocument());

            throw new NotImplementedException();
        }

        public void Push(IJob<IJobData> job)
        {
            throw new NotImplementedException();
        }
    }
}
