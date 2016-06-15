using BackOffice.Common;
using BackOffice.Jobs.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace BackOffice.Worker
{
    public class WorkerService
    {
        private readonly IJobQueue jobQueue;
        private readonly JobHandler handler;
        private readonly List<Task> tasks;

        public WorkerService(IJobQueue jobQueue, JobHandler handler)
        {
            this.jobQueue = jobQueue;
            this.handler = handler;
            this.tasks = new List<Task>();
        }

        public void Start()
        {
            Logging.Log().Debug("Waiting for upcoming jobs...");

            var newTask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        //remove this later
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
                        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                        var job = this.jobQueue.Pull();

                        if (job != null)
                        {
                            Logging.Log().Information("Found job to process. {job}", job);

                            try
                            {
                                this.handler.Handle(job);
                                this.jobQueue.SetJobStatus(job, JobStatus.Done);
                            }
                            catch (Exception ex)
                            {
                                Logging.Log().Warning("Job {job} failed! {ex}", job, ex);
                                this.jobQueue.SetJobStatus(job, JobStatus.Failed, ex.ToString());
                            }
                        }
                        else
                        {
                            //await Task.Delay(1000);
                            Logging.Log().Debug("No job found to process. Waiting for the next one.");
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log().Warning(ex, "Exception occured while waiting for upcoming event.");
                    }
                }
            });

            newTask.Wait();
        }
    }
}
