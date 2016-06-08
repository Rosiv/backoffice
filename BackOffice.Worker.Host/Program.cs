using BackOffice.Common;
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
            container.AutoRegister();
        }
    }
}
