using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueOrFalse.Web;

public class KnowledgeModel : BaseModel
{
    public string UserName => _sessionUser.User == null
        ?  "Unbekannte(r)"
        :  _sessionUser.User.Name;

    public IList<GetAnswerStatsInPeriodResult> Last30Days = new List<GetAnswerStatsInPeriodResult>();
    public bool HasLearnedInLast30Days;

    public int ActivityPoints;
    public int ActivityLevel;
    public int ActivityPointsAtNextLevel;
    public int ActivityPointsTillNextLevel;
    public int ActivityPointsPercentageOfNextLevel;

    public int QuestionsCount;
    public int QuestionsCreatedCount;
    public int SetsCount;
    public int SetsCreatedCount;

    public KnowledgeSummary KnowledgeSummary = new KnowledgeSummary();
    public GetStreaksDaysResult StreakDays = new GetStreaksDaysResult();

    public IList<Date> Dates = new List<Date>();
    public IList<Date> DatesInNetwork = new List<Date>();
    public IList<Answer> AnswerRecent = new List<Answer>();
    public IList<TrainingDateModel> TrainingDates = new List<TrainingDateModel>();

    public User User = new User();
    public int ReputationRank;
    public int ReputationTotal;

    public IList<object> CatsAndSetsWish;

    public UIMessage Message;

    public IList<UserActivity> NetworkActivities;
    public IList<string> NetworkActivitiesActionString;

    public KnowledgeModel(string emailKey = null)
    {
        if (!String.IsNullOrEmpty(emailKey))
            if (R<ValidateEmailConfirmationKey>().IsValid(emailKey))
                Message = new SuccessMessage("Deine E-Mail-Ad­res­se ist nun bestätigt.");

        if(HttpContext.Current.Request["passwordSet"] != null)
            Message = new SuccessMessage("Du hast dein Passwort aktualisiert.");

        if (!IsLoggedIn)
        {
            FillWithSampleData();
            return;
        }

        User = R<UserRepo>().GetById(UserId);
        QuestionsCount = R<GetWishQuestionCountCached>().Run(UserId);
        SetsCount = R<GetWishSetCount>().Run(UserId);

        ActivityPoints = User.ActivityPoints;
        ActivityLevel = User.ActivityLevel;
        ActivityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(ActivityLevel);
        ActivityPointsTillNextLevel = ActivityPointsAtNextLevel - ActivityPoints;
        ActivityPointsPercentageOfNextLevel = ActivityPoints == 0 ? 0 : 100 * ActivityPoints / ActivityPointsAtNextLevel;

        QuestionsCreatedCount = Resolve<UserSummary>().AmountCreatedQuestions(UserId);
        SetsCreatedCount = Resolve<UserSummary>().AmountCreatedSets(UserId);

        var reputation = Resolve<ReputationCalc>().Run(User);
        ReputationRank = User.ReputationPos;
        ReputationTotal = reputation.TotalReputation;

        var msg = new RecalcProbabilitiesMsg {UserId = UserId};
        //Bus.Get().Publish(msg);

        KnowledgeSummary = KnowledgeSummaryLoader.Run(UserId);

        var getAnswerStatsInPeriod = Resolve<GetAnswerStatsInPeriod>();
        Last30Days = getAnswerStatsInPeriod.GetLast30Days(UserId);
        HasLearnedInLast30Days = Last30Days.Sum(d => d.TotalAnswers) > 0;
        StreakDays = R<GetStreaksDays>().Run(User);


        //GET WISH KNOWLEDGE INFO
        var categoryValuationIds = UserValuationCache.GetCategoryValuations(UserId)
            .Where(v => v.IsInWishKnowledge())
            .Select(v => v.CategoryId)
            .ToList();
        var categoriesWishPool = R<CategoryRepository>().GetByIds(categoryValuationIds);
        var categoriesWish = OrderCategoriesByQuestionCountAndLevel.Run(categoriesWishPool);

        var setValuationIds = R<SetValuationRepo>().GetByUser(UserId).Select(v => v.SetId).ToList();
        var setsWish = R<SetRepo>().GetByIds(setValuationIds).OrderByDescending(s => s.QuestionCount()).ToList(); // sorts by questioncount including private questions! Excluding them would also exclude private questions visible to user
        CatsAndSetsWish = SortSetsIntoListOfCategories.Run(categoriesWish, setsWish);



        //GET DATES information
        Dates = R<DateRepo>().GetBy(UserId, true);
        DatesInNetwork = R<GetDatesInNetwork>().Run(UserId);

        AnswerRecent = R<AnswerRepo>().GetUniqueByUser(UserId, amount: 15);

        //GET NETWORK ACTIVITY
        NetworkActivities = R<UserActivityRepo>().GetByUser(User, 8);

        //GET TRAINING DATES
        var tdTrainingDates = R<TrainingDateRepo>().GetUpcomingTrainingDates(7);
        foreach (var tdTrainingDate in tdTrainingDates)
        {
            TrainingDates.Add(new TrainingDateModel(tdTrainingDate));
        }
    }

    private void FillWithSampleData()
    {
        ActivityPoints = 3210;
        ActivityLevel = UserLevelCalculator.GetLevel(ActivityPoints);
        ActivityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(ActivityLevel);
        ActivityPointsTillNextLevel = ActivityPointsAtNextLevel - ActivityPoints;
        ActivityPointsPercentageOfNextLevel = ActivityPoints == 0 ? 0 : 100 * ActivityPoints / ActivityPointsAtNextLevel;

        QuestionsCount = 288;
        SetsCount = 12;
        User = new User {
            Name = "Unbekannte(r)",
            DateCreated = DateTime.Now.AddMonths(-11)
        };

        QuestionsCreatedCount = 13;
        SetsCreatedCount = 2;

        var reputation = new ReputationCalcResult
        {
            ForQuestionsCreated = 120,
            ForSetsCreated = 80,
            ForDatesCopied = 20,
            ForSetsInOtherWishknowledge = 40,
            ForQuestionsInOtherWishknowledge = 220
        };
        ReputationRank = 38;
        ReputationTotal = reputation.TotalReputation;

        KnowledgeSummary = new KnowledgeSummary
        (
            notLearned : 25,
            needsLearning : 44,
            needsConsolidation : 91,
            solid : 128
        );

        Last30Days = new List<GetAnswerStatsInPeriodResult>();
        int totalDayAnswers;
        var random = new Random();
        for (int i = 0; i < 30; i++)
        {
            totalDayAnswers = random.Next(0, 65);
            Last30Days.Add(new GetAnswerStatsInPeriodResult
            {
                DateTime = DateTime.Now.AddDays(-i),
                TotalAnswers = totalDayAnswers,
                TotalTrueAnswers = (int)Math.Ceiling((double)(random.Next(40,101) / (double)100) * totalDayAnswers)
            });
        }
        HasLearnedInLast30Days = Last30Days.Sum(d => d.TotalAnswers) > 0;
        StreakDays = new GetStreaksDaysResult
        {
            LongestStart = DateTime.Now.AddDays(-123),
            LongestEnd = DateTime.Now.AddDays(-80),
            LongestLength = 43,
            LastStart = DateTime.Now.AddDays(-12),
            LastEnd = DateTime.Now,
            LastLength = 12,
            TotalLearningDays = 214
        };


        var wishCategories = Sl.R<CategoryRepository>().GetByIds(652, 145, 6) ?? new List<Category>();
        var wishSets = Sl.R<SetRepo>().GetByIds(14, 20, 189) ?? new List<Set>();
        CatsAndSetsWish = SortSetsIntoListOfCategories.Run(wishCategories, wishSets);

        Dates = GetSampleDates.Run();
        DatesInNetwork = GetSampleDates.RunAgain();

        AnswerRecent = new List<Answer>();
        var questionsLearned = R<QuestionRepo>().GetMostViewed(9);
        for (int i = 0; i < questionsLearned.Count; i++)
        {
            AnswerRecent.Add(new Answer
            {
                Question = questionsLearned[i]
            });
        }

        NetworkActivities = GetSampleUserActivities.Run(User);

        TrainingDates = new List<TrainingDateModel>
        {
            new TrainingDateModel(new TrainingDate
            {
                DateTime = DateTime.Now.AddHours(4),
                AllQuestions = Sl.R<SetRepo>().GetById(17).Questions().Select(q => new TrainingQuestion {Question = q, IsInTraining = true}).ToList(),
                TrainingPlan = new TrainingPlan {Date = Dates[0]}
            }),
            new TrainingDateModel(new TrainingDate
            {
                DateTime = DateTime.Now.AddHours(24),
                AllQuestions = Sl.R<SetRepo>().GetById(20).Questions().Select(q => new TrainingQuestion {Question = q, IsInTraining = true}).ToList(),
                TrainingPlan = new TrainingPlan {Date = Dates[0]}
            }),
            new TrainingDateModel(new TrainingDate
            {
                DateTime = DateTime.Now.AddHours(57),
                AllQuestions = Sl.R<SetRepo>().GetById(45).Questions().Select(q => new TrainingQuestion {Question = q, IsInTraining = true}).ToList(),
                TrainingPlan = new TrainingPlan {Date = Dates[1]}
            }),
            new TrainingDateModel(new TrainingDate
            {
                DateTime = DateTime.Now.AddHours(71),
                AllQuestions = Sl.R<SetRepo>().GetById(65).Questions().Select(q => new TrainingQuestion {Question = q, IsInTraining = true}).ToList(),
                TrainingPlan = new TrainingPlan()
            })
        };
    }

}