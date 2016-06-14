using Nancy;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Serilog;

namespace BackOffice.Web.Host
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

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

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("views", "views"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts", "Scripts"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("js", "js"));
        }
    }
}
