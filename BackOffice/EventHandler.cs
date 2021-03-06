﻿using BackOffice.Common;
using BackOffice.Interfaces;
using BackOffice.Jobs.Interfaces;
using BackOffice.Rules;
using System;

namespace BackOffice
{
    public class EventHandler
    {
        private readonly IJobQueue queue;
        private readonly IJobQueue fallbackQueue;

        public EventHandler(IJobQueue queue, IJobQueue fallbackQueue)
        {
            this.queue = queue;
            this.fallbackQueue = fallbackQueue;
        }

        internal void Handle(IEvent upcomingEvent)
        {
            Logging.Log().Debug("Handling event {upcomingEvent}", upcomingEvent.Name);

            var rules = new IRule[] {
                new AProductsReportsRule(upcomingEvent),
                new BProductsReportsRule(upcomingEvent),
                new CProductsAlwaysFailingRule(upcomingEvent),
                new DProductsReportsRule(upcomingEvent)
            };

            Logging.Log().Information("=====----- Checking {i} rules -----=====", rules.Length);

            foreach (var rule in rules)
            {
                Logging.Log().Information("Checking rule {rule}...", rule);
                if (rule.IsApplicable())
                {
                    Logging.Log().Information("Rule {rule} is applicable for an event {event}", rule, upcomingEvent);

                    var jobs = rule.CreateJobs();

                    Logging.Log().Information("Rule {rule} has returned {i} jobs to enqueue.", rule, jobs.Count);

                    foreach (var job in jobs)
                    {
                        try
                        {
                            job.Status = JobStatus.Enqueued;

                            Logging.Log().Information("Pushing job {job} to queue...", job.Name);

                            this.queue.Push(job);
                        }
                        catch (Exception ex)
                        {
                            Logging.Log().Warning("Pushing job {job} to queue failed! The job will be sent to the fallback queue. Exception: {ex}", job.Name, ex);
                            this.fallbackQueue.Push(job);
                        }
                    }
                }
                Logging.Log().Information("=====----- Rule check finished -----=====");
            }
        }
    }
}
