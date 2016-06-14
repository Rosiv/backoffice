using BackOffice.Common;
using Nancy;

namespace BackOffice.Web
{
    public class JobsModule : NancyModule
    {
        private readonly JobMongoDbView view;

        public JobsModule(JobMongoDbView view)
            : base("/jobs")
        {
            this.view = view;

            this.Get["/"] = parameters =>
            {
                Logging.Log().Information("Invoked GET /jobs");

                var json = this.view.GetJobs();

                return ((Response)json).WithContentType("application/json");
            };
        }
    }
}
