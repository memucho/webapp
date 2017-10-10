﻿using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Web.Script.Serialization;
using Quartz;
using RollbarSharp;

namespace TrueOrFalse.Utilities.ScheduledJobs
{
    public class RemoveCategoryFromWishKnowledge : IJob
    {
        public const int IntervalInSeconds = 2;

        public void Execute(IJobExecutionContext context)
        {
            JobExecute.Run(scope =>
            {
                var successfullJobIds = new List<int>();
                var jobs = scope.R<JobQueueRepo>().GetRemoveCategoryFromWishKnowledge();
                var categoryUserPairs = new List<CategoryUserPair>();
                foreach (var job in jobs)
                {
                    var serializer = new JavaScriptSerializer();
                    var categoryUserIdPair = serializer.Deserialize<CategoryUserPair>(job.JobContent);
                    categoryUserPairs.Add(categoryUserIdPair);
                }
                foreach (var categoryUserPair in categoryUserPairs)
                {
                    try
                    {
                        //DO THE JOB THING INSIDE HERE
                        //scope.R<ReputationUpdate>().Run(scope.R<UserRepo>().GetById(Convert.ToInt32(userJobs.Key)));
                        //successfullJobIds.AddRange(userJobs.Select(j => j.Id).ToList<int>());
                    }
                    catch (Exception e)
                    {
                        Logg.r().Error(e, "Error in job RemoveCategoryFromWishKnowledge.");
                        new RollbarClient().SendException(e);
                    }
                }

                //Delete jobs that have been executed successfully
                if (successfullJobIds.Count > 0)
                {
                    scope.R<JobQueueRepo>().DeleteById(successfullJobIds);
                    Logg.r().Information("Job RemoveCategoryFromWishKnowledge removed for " + successfullJobIds.Count + " jobs.");
                    successfullJobIds.Clear();
                }

            }, "RecalcReputation");
        }
    }
}
