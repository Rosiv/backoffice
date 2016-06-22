using BackOffice.Common;
using Nancy;

namespace BackOffice.Web
{
    public class JobsModule : NancyModule
    {
        public JobsModule(JobMongoService mongoService)
            : base("/jobs")
        {
            this.Get["/"] = parameters =>
            {
                Logging.Log().Information("Invoked GET /jobs");

                var json = mongoService.GetJobs();

                return ((Response)json).WithContentType("application/json");
            };

            this.Post["/{id}/retry"] = parameters =>
            {
                string jobId = (string)parameters.id;

                Logging.Log().Information("Invoked GET /jobs/" + jobId);

                mongoService.Retry(jobId);

                return HttpStatusCode.OK;
            };
        }
    }
}
