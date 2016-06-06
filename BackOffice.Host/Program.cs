using Serilog;
using System;
using TinyIoC;

namespace BackOffice.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterDependencies();
            Log.Information("Hello from BackOffice!");

            try
            {

            }
            catch(Exception ex)
            {
                Log.Error(ex, "Critical exception occured. Application will close.");
            }
        }

        static void RegisterDependencies()
        {
            var container = TinyIoCContainer.Current;
            container.AutoRegister();

            Log.Logger = new LoggerConfiguration()
                        .WriteTo.ColoredConsole()
                        .CreateLogger();

        }
    }
}
