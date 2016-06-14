using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;

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

            var docs = collection.Find(filter).ToList();

            docs.ForEach(doc => doc.Remove("_id"));

            if (docs != null)
            {
                var json = docs.ToJson();

                return json.ToString();
            }

            return new BsonDocument().ToJson();
        }
    }
}
