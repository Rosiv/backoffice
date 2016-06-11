using BackOffice.Common;
using BackOffice.Interfaces;
using BackOffice.Jobs.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TinyIoC;

namespace BackOffice
{
    public class BackOfficeService
    {
        private readonly EventHandler eventHandler;
        private readonly IJobQueue fallbackQueue;
        private readonly IJobQueue mainJobQueue;
        private readonly IEnumerable<IEventProvider> eventProviders;
        private readonly List<Task> tasks;

        public BackOfficeService(EventHandler eventHandler, IJobQueue mainJobQueue, IJobQueue fallbackQueue)
        {
            this.eventHandler = eventHandler;
            this.fallbackQueue = fallbackQueue;
            this.mainJobQueue = mainJobQueue;
            var container = TinyIoCContainer.Current;
            this.eventProviders = container.ResolveAll<IEventProvider>();
            this.tasks = new List<Task>();
        }

        public void Start()
        {
            Logging.Log().Debug("Waiting for upcoming events from the following providers:");

            foreach (var provider in this.eventProviders)
            {
                Logging.Log().Debug(provider.GetType().ToString());

                var newTask = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        try
                        {
                            //remove this later
                            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
                            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                            var newEvent = provider.Next();

                            this.eventHandler.Handle(newEvent);
                        }
                        catch (Exception ex)
                        {
                            Log.Warning(ex, "Exception occured while waiting for upcoming event.");
                        }
                    }
                });

                this.tasks.Add(newTask);
            }

            var recoveryTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var count = this.fallbackQueue.Count;

                    Logging.Log().Information("There is {i} job(s) in fallback queue.", count);

                    if (count > 0)
                    {
                        var job = this.fallbackQueue.Pull();

                        try
                        {
                            Logging.Log().Debug("Pushing job {job} to queue...", job.Name);
                            this.mainJobQueue.Push(job);
                        }
                        catch (Exception ex)
                        {
                            Logging.Log().Warning("Pushing job {job} to queue failed! The job will be sent to the fallback queue. Exception: {ex}", job.Name, ex);
                            this.fallbackQueue.Push(job);
                        }
                    }
                    else
                    {
                        await Task.Delay(10000);
                    }
                }
            });

            this.tasks.Add(recoveryTask);

            Task.WaitAll(this.tasks.ToArray());
        }
    }
}
