using BackOffice.Jobs.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackOffice.Web
{
    public class JobMongoService
    {
        private const string ConnectionString = "mongodb://localhost:27017/";
        private const string DatabaseName = "backoffice";
        private const string CollectionName = "job_queue";
        private readonly MongoClient client;

        public JobMongoService()
        {
            this.client = new MongoClient(ConnectionString);
        }

        public string GetJobs()
        {
            var db = this.client.GetDatabase(DatabaseName);
            var collection = db.GetCollection<BsonDocument>(CollectionName);
            var filter = new BsonDocument();
            var sort = Builders<BsonDocument>.Sort.Descending("_id");

            var docs = collection.Find(filter).Sort(sort).ToList();

            foreach (var doc in docs)
            {
                string id = doc.GetElement("_id").Value.ToString();
                doc.Remove("_id");
                doc.Add("JobId", id);
            }

            if (docs != null)
            {
                var json = docs.ToJson();

                return json.ToString();
            }

            return new BsonDocument().ToJson();
        }

        public void Retry(string jobId)
        {
            var db = this.client.GetDatabase(DatabaseName);
            var collection = db.GetCollection<BsonDocument>(CollectionName);
            var id = new ObjectId(jobId);
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var statusFilter = Builders<BsonDocument>.Filter.Eq("Status", JobStatus.Failed.ToString());
            var filter = Builders<BsonDocument>.Filter.And(idFilter, statusFilter);
            var update = Builders<BsonDocument>.Update.Set("Status", JobStatus.Enqueued.ToString());

            var x = collection.UpdateOne(filter, update);
        }
    }
}
