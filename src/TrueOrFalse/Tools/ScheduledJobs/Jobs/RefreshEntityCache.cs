using Quartz;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RefreshEntityCache : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope => 
            {
                EntityCache.Init(" (in JobScheduler) ");
            }, "RefreshEntityCache");
        }

    }
}