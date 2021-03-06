﻿using BackOffice.Common;
using BackOffice.Jobs.Interfaces;
using BackOffice.Jobs.Queues.MongoDB;
using BackOffice.Worker;
using Serilog;
using System;
using TinyIoC;

namespace BackOffice.Worker.Host
{
    class Program
    {
        private static TinyIoCContainer container = TinyIoCContainer.Current;

        static void Main(string[] args)
        {
            RegisterDependencies();
            Logging.Log().Information("Hello from BackOffice Worker!");

            try
            {
                var service = new WorkerService(container.Resolve<IJobQueue>(), container.Resolve<JobHandler>());
                service.Start();
            }
            catch (Exception ex)
            {
                Logging.Log().Error(ex, "Critical exception occured. Application will close.");
            }

            Logging.Log().Information("I am going to quit this neverending job...");
        }

        static void RegisterDependencies()
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .Enrich.FromLogContext()
                        .Enrich.WithProperty("CallerFilePath", string.Empty)
                        .Enrich.WithProperty("CallerMemberName", string.Empty)
                        .Enrich.WithProperty("CallerLineNumber", string.Empty)
                        .Enrich.WithProperty("ApplicationName", string.Empty)
                        .WriteTo.ColoredConsole(outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] [{CallerFilePath:l}] {Message}{NewLine}{Exception}")
                        .CreateLogger();

            container.Register(Log.Logger);
            container.Register<IJobQueue>(new MongoDBJobQueue());
            container.Register(new JobHandler());
        }
    }
}
