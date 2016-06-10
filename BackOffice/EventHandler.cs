using BackOffice.Common;
using BackOffice.Interfaces;
using BackOffice.Jobs.Interfaces;
using BackOffice.Rules;

namespace BackOffice
{
    public class EventHandler
    {
        private readonly IJobQueue queue;

        public EventHandler(IJobQueue queue)
        {
            this.queue = queue;
        }

        internal void Handle(IEvent upcomingEvent)
        {
            Logging.Log().Debug("Handling event {upcomingEvent}", upcomingEvent.Name);

            var rules = new[] {
                new ProductAInsertedRule(upcomingEvent)
            };

            Logging.Log().Debug("Checking {i} rules...", rules.Length);

            foreach (var rule in rules)
            {
                Logging.Log().Debug("Checking rule {rule}...", rule);
                if (rule.IsApplicable())
                {
                    Logging.Log().Debug("Rule {rule} is applicable for an event {event}", rule, upcomingEvent);

                    var jobs = rule.CreateJobs();

                    Logging.Log().Debug("Rule {rule} has returned {i} jobs to enqueue.", jobs.Count);

                    foreach (var job in jobs)
                    {
                        Logging.Log().Debug("Pushing job {job} to queue...", job.Name);

                        this.queue.Push(job);
                    }
                }
            }
        }
    }
}
