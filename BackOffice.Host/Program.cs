using BackOffice.Common;
using BackOffice.EventProviders.SqlEventProvider;
using Serilog;
using System;
using System.Threading.Tasks;
using TinyIoC;

namespace BackOffice.Host
{
    class Program
    {
        private static TinyIoCContainer container = TinyIoCContainer.Current;

        static void Main(string[] args)
        {
            RegisterDependencies();
            Logging.Log().Information("Hello from BackOffice!");

            try
            {
                var service = new BackOfficeService(container.Resolve<EventHandler>());
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
            container.AutoRegister();
            container.Register(new SqlEventProvider());
            container.Register(new TaskFactory());

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
        }
    }
}
