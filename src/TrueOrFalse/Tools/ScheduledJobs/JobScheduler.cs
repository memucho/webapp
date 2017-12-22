using Quartz;
using Quartz.Impl;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public static class JobScheduler
    {
        static readonly IScheduler _scheduler;

        static JobScheduler()
        {
            var container = AutofacWebInitializer.Run();

            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
            _scheduler.JobFactory = new AutofacJobFactory(container);
            _scheduler.Start();
        }

        public static void EmptyMethodToCallConstructor()
        {
        }

        public static void Shutdown()
        {
            _scheduler.Shutdown(waitForJobsToComplete:true);
        }

        public static void Start()
        {
            Sl.R<RunningJobRepo>().TruncateTable();
            
            Schedule_CleanupWorkInProgressQuestions();
            Schedule_GameLoop();
            Schedule_RecalcKnowledgeStati();
            Schedule_RecalcKnowledgeSummariesForCategory();
            Schedule_RecalcReputation();
            Schedule_RecalcReputationForAll();
            Schedule_TrainingReminderCheck();
            Schedule_TrainingPlanUpdateCheck();
            Schedule_EditCategoryInWishKnowledge();
            Schedule_KnowledgeReportCheck();
            Schedule_LOM_Export();
        }

        private static void Schedule_CleanupWorkInProgressQuestions()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<CleanUpWorkInProgressQuestions>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInHours(6)
                    .RepeatForever()).Build());
        }

        private static void Schedule_GameLoop()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<GameLoop>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(1)
                    .RepeatForever()).Build());
        }

        private static void Schedule_RecalcKnowledgeStati()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<RecalcKnowledgeStati>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x => 
                        x.StartingDailyAt(new TimeOfDay(2, 00))
                         .OnEveryDay()
                         .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_RecalcKnowledgeSummariesForCategory()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<RecalcKnowledgeSummariesForCategory>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(RecalcReputation.IntervalInSeconds)
                        .RepeatForever()).Build());
        }

        private static void Schedule_RecalcReputation()
        {
            //recalculates reputation for users specified in table jobqueue, which is filled when relevant actions are taken that affect users reputation
            _scheduler.ScheduleJob(JobBuilder.Create<RecalcReputation>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(RecalcReputation.IntervalInSeconds)
                    .RepeatForever()).Build());
        }

        private static void Schedule_RecalcReputationForAll()
        {
            //once a day, recalculate reputation for all users
            _scheduler.ScheduleJob(JobBuilder.Create<RecalcReputationForAll>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.StartingDailyAt(new TimeOfDay(3, 00))
                            .OnEveryDay()
                            .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_TrainingPlanUpdateCheck()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<TrainingPlanUpdateCheck>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x =>
                        x.WithIntervalInMinutes(TrainingPlanUpdateCheck.IntervalInMinutes)
                        .RepeatForever())
                    .Build());
        }

        private static void Schedule_TrainingReminderCheck()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<TrainingReminderCheck>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(x => 
                        x.WithIntervalInMinutes(TrainingReminderCheck.IntervalInMinutes)
                        .RepeatForever()).Build());
        }

        private static void Schedule_KnowledgeReportCheck()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<KnowledgeReportCheck>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.StartingDailyAt(new TimeOfDay(10, 00))
                            .OnEveryDay()
                            .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_LOM_Export()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<LomExportJob>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.StartingDailyAt(new TimeOfDay(3, 30))
                            .OnEveryDay()
                            .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_RefreshEntityCache()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<RefreshEntityCache>().Build(),
                TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule(x =>
                        x.StartingDailyAt(new TimeOfDay(3, 30))
                            .OnEveryDay()
                            .EndingDailyAfterCount(1)).Build());
        }

        private static void Schedule_EditCategoryInWishKnowledge()
        {
            _scheduler.ScheduleJob(JobBuilder.Create<EditCategoryInWishKnowledge>().Build(),
                TriggerBuilder.Create().
                    WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(EditCategoryInWishKnowledge.IntervalInSeconds)
                            .RepeatForever()).Build());
        }

        public static void StartImmediately_TrainingReminderCheck() { StartImmediately<TrainingReminderCheck>(); }
        public static void StartImmediately_TrainingPlanUpdateCheck() { StartImmediately<TrainingPlanUpdateCheck>(); }
        public static void StartImmediately_CleanUpWorkInProgressQuestions() { StartImmediately<CleanUpWorkInProgressQuestions>(); }
        public static void StartImmediately_RecalcKnowledgeStati() { StartImmediately<RecalcKnowledgeStati>(); }
        public static void StartImmediately_RefreshEntityCache() { StartImmediately<RefreshEntityCache>(); }

        public static void StartImmediately<TypeToStart>() where TypeToStart : IJob
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<TypeToStart>().Build(),
                TriggerBuilder.Create().StartNow().Build());
        }

        public static void StartImmediately_InitUserValuationCache(int userId)
        {
            _scheduler.ScheduleJob(
                JobBuilder.Create<InitUserValuationCache>()
                .UsingJobData("userId", userId)
                .Build(),
                TriggerBuilder.Create().StartNow().Build());
        }
    }
}

