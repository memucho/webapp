using System;
using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class UpdateAnswerAggregatesJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var startedAt = DateTime.Now;

                UpdateAnswerAggregates.FullUpadte();

                Sl.JobHistoryRepo.AddUpdateAnswerAggregates(startedAt);

            }, nameof(UpdateAnswerAggregates));
        }
    }
}