using BackOffice.Jobs.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;
using BackOffice.Common;
using BackOffice.Jobs.Common;

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
            var filter = Builders<BsonDocument>.Filter.Eq("Status", JobStatus.Enqueued.ToString());
            var update = Builders<BsonDocument>.Update.Set("Status", JobStatus.InProgress.ToString());

            var doc = collection.FindOneAndUpdate(filter, update);

            if (doc != null)
            {
                doc.Remove("_id");
                var jsonJob = doc.ToJson();
                var job = JobSerializer.Deserialize(jsonJob);

                return job;
            }

            return null;
        }

        public void Push(IJob<IJobData> job)
        {
            BsonDocument document = JobSerializer.ToBsonDocument(job);

            Logging.Log().Debug("Inserting job into MongoDB queue: {document}", JobSerializer.ToJson(job));

            var db = this.client.GetDatabase(DatabaseName);
            var collection = db.GetCollection<BsonDocument>(CollectionName);

            collection.InsertOne(document);

            Logging.Log().Debug("{document} inserted into MongoDB queue.", document);
        }

        public long Count
        {
            get
            {
                var db = this.client.GetDatabase(DatabaseName);
                var collection = db.GetCollection<BsonDocument>(CollectionName);

                var count = collection.Count(new BsonDocument());

                return count;
            }
        }
    }
}
