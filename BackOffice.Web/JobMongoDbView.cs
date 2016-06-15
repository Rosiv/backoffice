using MongoDB.Bson;
using MongoDB.Driver;

namespace BackOffice.Web
{
    public class JobMongoDbView
    {
        private const string ConnectionString = "mongodb://localhost:27017/";
        private const string DatabaseName = "backoffice";
        private const string CollectionName = "job_queue";
        private readonly MongoClient client;

        public JobMongoDbView()
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
    }
}
