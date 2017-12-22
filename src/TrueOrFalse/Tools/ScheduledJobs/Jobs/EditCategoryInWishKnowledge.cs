using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Autofac;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class EditCategoryInWishKnowledge : IJob
    {
        public const int IntervalInSeconds = 2;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var allJobs = new List<JobQueue>();
                allJobs.AddRange(scope.R<JobQueueRepo>().GetAddCategoryToWishKnowledge());
                allJobs.AddRange(scope.R<JobQueueRepo>().GetRemoveQuestionsInCategoryFromWishKnowledge());
                allJobs = allJobs.OrderBy(j => j.Id).ToList();

                foreach (var job in allJobs)
                {
                    switch (job.JobQueueType)
                    {
                        case JobQueueType.AddCategoryToWishKnowledge:
                            AddCategoryToWishKnowledge(job, scope);
                            break;

                        case JobQueueType.RemoveQuestionsInCategoryFromWishKnowledge:
                            RemoveQuestionsInCategoryFromWishKnowledge(job, scope);
                            break;
                    }
                }
            }, "EditCategoryInWishKnowledge");
        }

        private static void AddCategoryToWishKnowledge(JobQueue job, ILifetimeScope scope)
        {
            try
            {
                var categoryUserPair = GetCategoryUserPair(job);
                CategoryInKnowledge.PinCategoryInDatabase(categoryUserPair.CategoryId, categoryUserPair.UserId);

                scope.R<JobQueueRepo>().Delete(job.Id);
                Logg.r().Information($"Job EditCategoryInWishKnowledge added QuestionValuations for Category { categoryUserPair.CategoryId } and User { categoryUserPair.UserId }");
            }
            catch (Exception e)
            {
                Logg.r().Error(e, "Error in job EditCategoryInWishKnowledge.");
                new RollbarClient().SendException(e);
            }
        }

        private static void RemoveQuestionsInCategoryFromWishKnowledge(JobQueue job, ILifetimeScope scope)
        {
            try
            {
                var categoryUserPair = GetCategoryUserPair(job);
                CategoryInKnowledge.UnpinQuestionsInCategoryInDatabase(categoryUserPair.CategoryId, categoryUserPair.UserId);

                scope.R<JobQueueRepo>().Delete(job.Id);
                Logg.r().Information($"Job EditCategoryInWishKnowledge removed QuestionValuations for Category { categoryUserPair.CategoryId } and User { categoryUserPair.UserId }");
            }
            catch (Exception e)
            {
                Logg.r().Error(e, "Error in job EditCategoryInWishKnowledge.");
                new RollbarClient().SendException(e);
            }
        }

        private static CategoryUserPair GetCategoryUserPair(JobQueue jobQueueEntry)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<CategoryUserPair>(jobQueueEntry.JobContent);
        }
    }

    public class CategoryUserPair
    {
        public int CategoryId;
        public int UserId;
    }
}
