using BackOffice.Common;
using BackOffice.Jobs.Dto;
using BackOffice.Jobs.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using System;

namespace BackOffice.Jobs.Common
{
    public class JobSerializer
    {
        public static string ToJson(IJob<IJobData> job)
        {
            string json = JsonConvert.SerializeObject(job);
            return json;
        }

        public static BsonDocument ToBsonDocument(IJob<IJobData> job)
        {
            string json = ToJson(job);
            BsonDocument document = ToBsonDocument(json);

            return document;
        }

        public static BsonDocument ToBsonDocument(string json)
        {
            BsonDocument document = BsonSerializer.Deserialize<BsonDocument>(json);

            return document;
        }

        public static IJob<IJobData> Deserialize(string json)
        {
            try
            {
                var bson = ToBsonDocument(json);
                string typeName = bson["Type"].ToString();
                Type type = Type.GetType(typeName);

                var job = (IJob<IJobData>)JsonConvert.DeserializeObject(json, type);

                return job;
            }
            catch (Exception ex)
            {
                Logging.Log().Warning("Failed to deserialize job. {ex}", ex);
            }

            return null;
        }
    }
}
