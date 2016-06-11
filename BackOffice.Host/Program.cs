using BackOffice.Common;
using BackOffice.EventProviders.SqlEventProvider;
using BackOffice.Interfaces;
using BackOffice.Jobs.Queues.FileSystem;
using BackOffice.Jobs.Queues.MongoDB;
using Serilog;
using System;
using System.Globalization;
using System.Threading;
using TinyIoC;

namespace BackOffice.Host
{
    class Program
    {
        private static TinyIoCContainer container = TinyIoCContainer.Current;
        private const string FallbackQueuePath = "FallbackQueue";

        static void Main(string[] args)
        {
            //remove this later
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            RegisterDependencies();
            Logging.Log().Information("Hello from BackOffice!");

            try
            {
                var service = container.Resolve<BackOfficeService>();
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
            container.Register<IEventProvider>(new SqlEventProvider());
            var mainQueue = new MongoDBJobQueue();
            var fallbackQueue = new FileSystemQueue(FallbackQueuePath);

            
            var eventHandler = new EventHandler(mainQueue, fallbackQueue);
            container.Register(eventHandler);
            var service = new BackOfficeService(eventHandler, mainQueue, fallbackQueue);
            container.Register(service);
        }
    }
}
