using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class UpdateAnswerAggregatesJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => UpdateAnswerAggregates.FullUpadte(), nameof(UpdateAnswerAggregates));
        }
    }
}