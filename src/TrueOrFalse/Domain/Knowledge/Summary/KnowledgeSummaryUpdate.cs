class KnowledgeSummaryUpdate
{
    public static void RunForCategory(int catgoryId)
    {
        var catValuationRepo = Sl.CategoryValuationRepo;

        foreach (var categoryValuation in catValuationRepo.GetByCategory(catgoryId))
        {
            Run(categoryValuation);
        }
    }

    public static void RunForUser(int userId)
    {
        var catValuationRepo = Sl.CategoryValuationRepo;

        foreach (var categoryValuation in catValuationRepo.GetByUser(userId))
        {
            Run(categoryValuation);
        }
    }

    public static void Run(CategoryValuation categoryValuation, bool persist = true)
    {
        var knowledgeSummary = KnowledgeSummaryLoader.Run(categoryValuation.UserId, categoryValuation.CategoryId, false);
        categoryValuation.CountNotLearned = knowledgeSummary.NotLearned;
        categoryValuation.CountNeedsLearning = knowledgeSummary.NeedsLearning;
        categoryValuation.CountNeedsConsolidation = knowledgeSummary.NeedsConsolidation;
        categoryValuation.CountSolid = knowledgeSummary.Solid;

        if(!persist) return;

        Sl.CategoryValuationRepo.Update(categoryValuation);
    }

    public static void ScheduleForCategory(int categoryId) 
        => Sl.R<JobQueueRepo>().Add(JobQueueType.RecalcKnowledgeSummaryForCategory, categoryId.ToString());

    
}
