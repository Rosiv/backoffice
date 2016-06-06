using Serilog;
using TinyIoC;

namespace BackOffice.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterDependencies();
            Log.Information("Hello");
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
