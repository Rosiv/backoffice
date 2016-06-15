using BackOffice.Common;
using BackOffice.Jobs.Common;
using BackOffice.Jobs.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BackOffice.Jobs.Queues.FileSystem
{
    public class FileSystemQueue : IJobQueue
    {
        private readonly string queuePath;
        private readonly Guid queueGuid;
        private readonly Dictionary<Guid, string> jobs;

        public FileSystemQueue(string queuePath)
        {
            this.jobs = new Dictionary<Guid, string>();
            this.queueGuid = Guid.NewGuid();
            this.queuePath = Path.Combine(queuePath, this.queueGuid.ToString());

            if (!Directory.Exists(this.queuePath))
            {
                Directory.CreateDirectory(this.queuePath);
            }

            //write permissions test
            var testFilePath = Path.Combine(this.queuePath, "test.txt");
            File.WriteAllText(testFilePath, "write permissions test");
            File.Delete(testFilePath);
        }

        public IJob<IJobData> Pull()
        {
            if (jobs.Count > 0)
            {
                var jsonJob = jobs.First();
                var job = JobSerializer.Deserialize(jsonJob.Value);

                var jobFilePath = Path.Combine(this.queuePath, jsonJob.Key + ".json");
                File.Delete(jobFilePath);

                jobs.Remove(jsonJob.Key);

                return job;
            }

            return null;
        }

        public void Push(IJob<IJobData> job)
        {
            Guid newGuid;
            do
            {
                newGuid = Guid.NewGuid();
            }
            while (jobs.ContainsKey(newGuid));

            string json = JobSerializer.ToJson(job);

            Logging.Log().Debug("Inserting job into FileSystem queue: {json}", json);

            this.jobs.Add(newGuid, json);

            string path = Path.Combine(this.queuePath, newGuid.ToString() + ".json");
            File.WriteAllText(path, json);
        }

        public void SetJobStatus(IJob<IJobData> job, JobStatus status, string additionalInfo = null)
        {
            throw new NotImplementedException();
        }

        public long Count
        {
            get
            {
                return this.jobs.Count;
            }
        }
    }
}
