using BackOffice.Common;
using BackOffice.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyIoC;

namespace BackOffice
{
    public class BackOfficeService
    {
        private readonly EventHandler eventHandler;
        private readonly IEnumerable<IEventProvider> eventProviders;
        private readonly List<Task> tasks;

        public BackOfficeService(EventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
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

            Task.WaitAll(this.tasks.ToArray());
        }
    }
}
