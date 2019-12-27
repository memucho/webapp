using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Search;
using TrueOrFalse.Utilities.ScheduledJobs;
using TrueOrFalse.Web;
using static System.String;

[AccessOnlyAsAdmin]
[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
[SetUserMenu(UserMenuEntry.None)]
public class MaintenanceController : BaseController
{
    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Maintenance()
    {
        return View(new MaintenanceModel());
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Messages()
    {
        return View(new MessagesModel());
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult CMS()
    {
        var settings = Sl.R<DbSettingsRepo>().Get();
        return View(new CMSModel
            {SuggestedGames = settings.SuggestedGames, SuggestedSetsIdString = settings.SuggestedSetsIdString}.Init());
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CMS(CMSModel cmsModel)
    {
        cmsModel.ConsolidateGames();
        cmsModel.ConsolidateSuggestedSets();
        cmsModel.Message = new SuccessMessage("CMS Werte gespeichert");

        ModelState.Clear();

        return View(cmsModel);
    }

    public string CmsRenderCategoryNetworkNavigation(int id)
    {
        return ViewRenderer.RenderPartialView(
            "~/Views/Categories/Navigation/CategoryNetworkNavigation.ascx",
            new CategoryNetworkNavigationModel(id),
            ControllerContext);
    }

    [HttpPost]
    public string CmsRenderLooseCategories()
    {
        var looseCategories = GetAllCategoriesUnconnectedToRootCategories.Run()
            .OrderByDescending(c => c.CountQuestionsAggregated);
        var result = looseCategories.Count() +
                     " categories found (ordered by aggregated question count descending):<br/>";
        foreach (var category in looseCategories)
        {
            result += ViewRenderer.RenderPartialView("~/Views/Shared/CategoryLabel.ascx", category, ControllerContext);
        }

        return result;
    }

    [HttpPost]
    public string CmsRenderCategoriesWithNonAggregatedChildren()
    {
        var categories = Sl.CategoryRepo.GetAllEager();
        categories = categories.Where(c => c.NonAggregatedCategories().Any()).ToList();

        var result = categories.Count() + " categories found:<br/>";
        foreach (var category in categories)
        {
            result += ViewRenderer.RenderPartialView("~/Views/Shared/CategoryLabel.ascx", category, ControllerContext);
        }

        return result;
    }

    [HttpPost]
    public string CmsRenderCategoriesInSeveralRootCategories()
    {
        var doubleRootedCategories = GetAllCategoriesInSeveralRootCategories.Run()
            .OrderByDescending(c => c.CountQuestionsAggregated);
        var result = doubleRootedCategories.Count() +
                     " categories found (ordered by aggregated question count descending):<br/>";
        foreach (var category in doubleRootedCategories)
        {
            result += ViewRenderer.RenderPartialView("~/Views/Shared/CategoryLabel.ascx", category, ControllerContext);
        }

        return result;
    }

    [HttpPost]
    public string CmsRenderOvercategorizedSets()
    {
        var sets = GetAllOvercategorizedSets.Run();
        var result = sets.Count() + " sets were found:<br/>";
        foreach (var set in sets)
        {
            result += "<a href=\"" + Links.SetDetail(Url, set) +
                      "\"><span class=\"label label-set\" style=\"max-width: 200px; margin-top: 5px; margin-right: 10px;\">" +
                      set.Id + "-" + set.Name + "</span></a> \n";
        }

        return result;
    }

    [HttpPost]
    public string CmsRenderSetsWithDifferentlyCategorizedQuestions()
    {
        var sets = GetAllSetsWithDifferentlyCategorizedQuestions.Run().OrderByDescending(s => s.DateCreated);
        var result = sets.Count() + " sets were found:<br/>";
        foreach (var set in sets)
        {
            result += "<a href=\"" + Links.SetDetail(Url, set) +
                      "\"><span class=\"label label-set\" style=\"max-width: 200px; margin-top: 5px; margin-right: 10px;\">" +
                      set.Id + "-" + set.Name + "</span></a> \n";
        }

        return result;
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult ContentCreatedReport()
    {
        return View(new ContentCreatedReportModel());
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult ContentStats()
    {
        return View(new ContentStatsModel());
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult ContentStatsShowStats()
    {
        return View("ContentStats", new ContentStatsModel(true));
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Statistics()
    {
        return View(new StatisticsModel());
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult RecalculateAllKnowledgeItems()
    {
        R<AddValuationEntries_ForQuestionsInSetsAndDates>().RunForAllUsers();

        ProbabilityUpdate_ValuationAll.Run();
        ProbabilityUpdate_Question.Run();
        ProbabilityUpdate_Category.Run();
        ProbabilityUpdate_User.Run();

        return View("Maintenance", new MaintenanceModel
        {
            Message = new SuccessMessage("Antwortwahrscheinlichkeiten wurden neu berechnet.")
        });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CalcAggregatedValuesQuestions()
    {
        Resolve<UpdateQuestionAnswerCounts>().Run();
        return View("Maintenance",
            new MaintenanceModel {Message = new SuccessMessage("Aggregierte Werte wurden aktualisiert.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CalcAggregatedValuesSets()
    {
        Resolve<UpdateSetDataForQuestion>().Run();
        return View("Maintenance",
            new MaintenanceModel {Message = new SuccessMessage("Aggregierte Werte wurden aktualisiert.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult DeleteValuationsForRemovedSets()
    {
        Resolve<DeleteValuationsForNonExisitingSets>().Run();
        return View("Maintenance",
            new MaintenanceModel {Message = new SuccessMessage("Valuations for deleted sets are removed.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult UpdateFieldQuestionCountForCategories()
    {
        Resolve<UpdateQuestionCountForCategory>().All();
        return View("Maintenance",
            new MaintenanceModel {Message = new SuccessMessage("Feld: AnzahlFragen für Themen wurde aktualisiert.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult UpdateUserReputationAndRankings()
    {
        Resolve<ReputationUpdate>().RunForAll();
        return View("Maintenance",
            new MaintenanceModel {Message = new SuccessMessage("Reputation and Rankings wurden aktualisiert.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult UpdateUserWishCount()
    {
        Resolve<UpdateWishcount>().Run();
        return View("Maintenance",
            new MaintenanceModel
                {Message = new SuccessMessage("Wunschwissen-Antwortwahrscheinlichkeit wurde aktualisiert.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult ReIndexAllQuestions()
    {
        Resolve<ReIndexAllQuestions>().Run();
        return View("Maintenance", new MaintenanceModel {Message = new SuccessMessage("Fragen wurden neu indiziert.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult ReIndexAllSets()
    {
        Resolve<ReIndexAllSets>().Run();
        return View("Maintenance",
            new MaintenanceModel {Message = new SuccessMessage("Lernsets wurden neu indiziert.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult ReIndexAllCategories()
    {
        Resolve<ReIndexAllCategories>().Run();
        return View("Maintenance", new MaintenanceModel {Message = new SuccessMessage("Themen wurden neu indiziert.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult ReIndexAllUsers()
    {
        Resolve<ReIndexAllUsers>().Run();
        return View("Maintenance", new MaintenanceModel {Message = new SuccessMessage("Nutzer wurden neu indiziert.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult SendMessage(MessagesModel model)
    {
        CustomMsg.Send(
            model.TestMsgReceiverId,
            model.TestMsgSubject,
            model.TestMsgBody);

        model.Message = new SuccessMessage("Message was sent");
        return View("Messages", model);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult SendKnowledgeReportMessage(MessagesModel model)
    {
        KnowledgeReportMsg.SendHtmlMail(_sessionUser.User);

        model.Message = new SuccessMessage("KnowledgeReport was sent to user <em>" + _sessionUser.User.Name +
                                           "</em> with email address <em>" + _sessionUser.User.EmailAddress + "</em>.");
        return View("Messages", model);
    }

    [SetMainMenu(MainMenuEntry.Maintenance)]
    public ActionResult Tools()
    {
        return View(new ToolsModel());
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult Throw500()
    {
        throw new Exception("Some random exception");
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CleanUpWorkInProgressQuestions()
    {
        JobScheduler.StartImmediately_CleanUpWorkInProgressQuestions();
        return View("Tools",
            new ToolsModel {Message = new SuccessMessage("Job: 'Cleanup work in progress' wird ausgeführt.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult TrainingReminderCheck()
    {
        //JobScheduler.StartImmediately_TrainingReminderCheck();
        return View("Tools",
            new ToolsModel {Message = new SuccessMessage("Job: 'Training Reminder Check' wird ausgeführt.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult TrainingPlanUpdateCheck()
    {
        //JobScheduler.StartImmediately_TrainingPlanUpdateCheck();
        Logg.r().Information("TrainingPlanUpdateCheck manually started");
        return View("Tools",
            new ToolsModel {Message = new SuccessMessage("Job: 'Training Plan Update Check' wird ausgeführt.")});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult Start100TestJobs()
    {
        for (var i = 0; i < 100; i++)
            JobScheduler.StartImmediately<TestJob1>();

        for (var i = 0; i < 100; i++)
            JobScheduler.StartImmediately<TestJob2>();

        return View("Tools", new ToolsModel {Message = new SuccessMessage("Started 100 test jobs.")});

    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult AssignCategoryToQuestionsInSet(ToolsModel toolsModel)
    {
        var categoryToAssign = Sl.R<CategoryRepository>().GetById(toolsModel.CategoryToAddId);

        var setIds = toolsModel.SetsToAddCategoryToIds.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x)).ToList();

        var sets = Sl.Resolve<SetRepo>().GetByIds(setIds);

        if (sets.Count == 0)
        {
            throw new Exception("no sets found");
        }

        var setsString = "";

        sets.ForEach(s => setsString += $", \"{s.Name}\" (Id {s.Id})");

        setsString = setsString.Substring(2); //Remove superfluous characters

        var questionRepo = Sl.R<QuestionRepo>();

        var questions = sets.SelectMany(s => s.Questions());

        foreach (var question in questions)
        {
            question.Categories.Add(categoryToAssign);
            question.Categories = question.Categories.Distinct().ToList();
            questionRepo.Update(question);
        }

        toolsModel.Message =
            new SuccessMessage(
                $"Das Thema \"{categoryToAssign.Name}\" (Id {categoryToAssign.Id}) wurde den Fragen in den Lernsets {setsString} zugewiesen");

        //ModelState.Clear(); 

        return View("Tools", toolsModel);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult RemoveCategoryFromQuestionsInSet(ToolsModel toolsModel)
    {
        var categoryToRemove = Sl.R<CategoryRepository>().GetById(toolsModel.CategoryToRemoveId);

        var setIds = toolsModel.SetsToRemoveCategoryFromIds.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToInt32(x)).ToList();

        var sets = Sl.Resolve<SetRepo>().GetByIds(setIds);

        if (sets.Count == 0)
        {
            throw new Exception("no sets found");
        }

        var setsString = "";

        sets.ForEach(s => setsString += $", \"{s.Name}\" (Id {s.Id})");

        setsString = setsString.Substring(2); //Remove superfluous characters

        var questionRepo = Sl.R<QuestionRepo>();

        var questions = sets.SelectMany(s => s.Questions());

        foreach (var question in questions)
        {
            question.Categories.Remove(categoryToRemove);
            question.Categories = question.Categories.Distinct().ToList();
            questionRepo.Update(question);
        }

        toolsModel.Message =
            new SuccessMessage(
                $"Das Thema \"{categoryToRemove.Name}\" (Id {categoryToRemove.Id}) wurde von den Fragen in den Lernsets {setsString} entfernt");

        return View("Tools", toolsModel);
    }

    [HttpPost]
    public ActionResult MigrateAnswerData()
    {
        var questionViewRepo = Sl.R<QuestionViewRepository>();

        var allQuestionViews = questionViewRepo.GetAll().ToList();

        var answerRepo = Sl.R<AnswerRepo>();

        var allAnswers = answerRepo.GetAll().OrderBy(x => x.DateCreated);

        var questionViewsToUpdate = allQuestionViews
            .Where(v => v.GuidString == null)
            .OrderBy(v => v.DateCreated)
            .ToList();

        var maxTimeForView = TimeSpan.FromHours(2);

        foreach (var questionView in questionViewsToUpdate)
        {
            questionView.Guid = Guid.NewGuid();
            questionView.Migrated = true;
            questionViewRepo.Update(questionView);

            IList<Answer> answersForQuestionView;

            if (questionView.Round != null)
            {

                answersForQuestionView = allAnswers.Where(a => a.Round == questionView.Round
                                                               && a.UserId == questionView.UserId)
                    .ToList();
            }
            else
            {

                var nextViewIndex = allQuestionViews.FindIndex(x =>
                    x.DateCreated > questionView.DateCreated
                    && x.UserId == questionView.UserId);

                var upperTimeBound =
                    nextViewIndex != -1 &&
                    allQuestionViews[nextViewIndex].DateCreated < questionView.DateCreated.Add(maxTimeForView)
                        ? allQuestionViews[nextViewIndex].DateCreated
                        : questionView.DateCreated.Add(maxTimeForView);

                answersForQuestionView = allAnswers
                    .Where(a =>
                        a.QuestionViewGuidString == null
                        && a.DateCreated >= questionView.DateCreated && a.DateCreated < upperTimeBound
                        && a.UserId == questionView.UserId
                        && a.Question.Id == questionView.QuestionId)

                    .ToList();
            }

            AddInteractionNumber(answersForQuestionView);

            foreach (var answer in answersForQuestionView)
            {
                answer.QuestionViewGuid = questionView.Guid;
                answer.Migrated = true;
                answer.MillisecondsSinceQuestionView = (answer.DateCreated - questionView.DateCreated).Milliseconds;

                answerRepo.Update(answer);
            }
        }

        return View("Maintenance", new MaintenanceModel {Message = new SuccessMessage("Wurde migriert")});
    }

    public void AddInteractionNumber(IList<Answer> answersForQuestionView)
    {
        var orderedAnswers = answersForQuestionView
            .OrderBy(a =>
            {
                switch (a.AnswerredCorrectly)
                {
                    case AnswerCorrectness.False:
                        return 0;
                    case AnswerCorrectness.True:
                        return 1;
                    case AnswerCorrectness.IsView:
                        return 3;
                    case AnswerCorrectness.MarkedAsTrue:
                        return
                            a.AnswerText == ""
                                ? 4
                                : 2; //MarkedAsTrue can be after IsView if newly created or before if existing answer is changed
                }

                return 3;
            })
            .ThenBy(a => a.DateCreated);

        var interactionNumber = 1;

        foreach (var answer in orderedAnswers)
        {
            answer.InteractionNumber = interactionNumber;
            interactionNumber++;
        }
    }

    [HttpPost]
    public ActionResult CheckForDuplicateInteractionNumbers()
    {
        var duplicates = Sl.R<AnswerRepo>().GetAll()
            .Where(a => a.QuestionViewGuid != Guid.Empty)
            .GroupBy(a => new {a.QuestionViewGuid, a.InteractionNumber})
            .Where(g => g.Skip(1).Any())
            .SelectMany(g => g)
            .ToList();

        var message = duplicates.Any() ? "Es gibt Dubletten." : "Es gibt keine Dubletten.";

        return View("Maintenance", new MaintenanceModel {Message = new SuccessMessage(message)});
    }

    [HttpPost]
    public ActionResult CheckForDuplicateLearningSessionStepGuidsInAnswers()
    {
        var duplicates = Sl.R<AnswerRepo>().GetAll()
            .Where(a => a.LearningSessionStepGuid != Guid.Empty)
            .GroupBy(a => new {a.LearningSessionStepGuid})
            .Where(g => g.Skip(1).Any())
            .SelectMany(g => g)
            .ToList();

        var message = duplicates.Any()
            ? $"Dubletten: {duplicates.Select(a => a.Id.ToString()).Aggregate((a, b) => a + " " + b)}"
            : "Es gibt keine Dubletten.";

        return View("Maintenance", new MaintenanceModel {Message = new SuccessMessage(message)});
    }

    [HttpPost]
    public ActionResult ClearMigratedData()
    {
        var questionViewRepo = Sl.R<QuestionViewRepository>();

        questionViewRepo.GetAll()
            .Where(v => v.Migrated)
            .ForEach(v =>
            {
                v.GuidString = null;
                v.Milliseconds = 0;
                questionViewRepo.Update(v);
            });

        var answerRepo = Sl.R<AnswerRepo>();

        answerRepo.GetAll()
            .Where(a => a.Migrated)
            .ForEach(a =>
            {
                a.QuestionViewGuidString = null;
                a.InteractionNumber = 0;
                a.MillisecondsSinceQuestionView = 0;
                answerRepo.Update(a);
            });

        return View("Maintenance", new MaintenanceModel {Message = new SuccessMessage("Cleared")});
    }

    [HttpPost]
    public ActionResult CheckForDuplicateGameRoundAnswers()
    {
        var duplicates = Sl.R<AnswerRepo>().GetAll()
            .Where(a => a.Round != null)
            .GroupBy(a => new {a.Round, a.UserId})
            .Where(g => g.Count(a => a.AnswerredCorrectly == AnswerCorrectness.IsView) > 1
                        || g.Count(a => a.AnsweredCorrectly()) > 1)
            .SelectMany(g => g)
            .ToList();

        var message = duplicates.Any()
            ? $"Dubletten: {duplicates.Select(a => a.Id.ToString()).Aggregate((a, b) => a + " " + b)}"
            : "Es gibt keine Dubletten.";

        return View("Maintenance", new MaintenanceModel {Message = new SuccessMessage(message)});
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CheckForCategoriesWithIncorrectQuestionCount()
    {
        var list = new List<Category>();

        var cats = Sl.R<CategoryRepository>().GetAll();
        var questionRepo = Sl.R<QuestionRepo>();

        foreach (var cat in cats)
        {
            if (cat.GetCountQuestionsAggregated() != cat.CountQuestionsAggregated)
                list.Add(cat);
        }

        return View("Maintenance", new MaintenanceModel { });
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult CreateAggregationsForAll()
    {
        var allCategories = Sl.CategoryRepo.GetAll();

        foreach (var category in allCategories)
        {
            Logg.r().Information("Created aggregates for {0}", category.Name);
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category);
        }

        return View("Maintenance", new MaintenanceModel {Message = new SuccessMessage("Aggregate erstellt")});
    }


    [HttpPost]
    public ActionResult MigrateDefaultTemplates()
    {
        var allCategories = Sl.CategoryRepo.GetAll();

        foreach (var category in allCategories)
        {
            if (IsNullOrWhiteSpace(category.TopicMarkdown))
            {
                var topicMarkdownBeforeUpdate = category.TopicMarkdown;
                var categoryId = category.Id;

                if (R<CategoryRepository>().GetChildren(category.Id)
                    .Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard))
                    category.TopicMarkdown = "[[{\"TemplateName\":\"TopicNavigation\",\"Title\":\"Unterthemen\"}]]\r\n";

                var aggregatedSets = category.GetAggregatedSetsFromMemoryCache();
                var aggregatedSetsCount = aggregatedSets.Count;
                var aggregatedQuestionCount = category.GetCountQuestionsAggregated();

                if (aggregatedSetsCount > 0 && aggregatedSetsCount <= 5)
                {
                    foreach (var set in aggregatedSets)
                        category.TopicMarkdown = category.TopicMarkdown +
                                                 "[[{\"TemplateName\":\"SingleSetFullWidth\",\"SetId\":" + set.Id +
                                                 "}]]" + Environment.NewLine;
                }
                else if (aggregatedSetsCount == 0 && aggregatedQuestionCount > 0)
                    category.TopicMarkdown = category.TopicMarkdown + "[[{\"TemplateName\":\"SingleQuestionsQuiz\"}]]" +
                                             Environment.NewLine;

                if (R<CategoryRepository>().GetChildren(category.Id)
                    .Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Education))
                    category.TopicMarkdown = category.TopicMarkdown + "[[{\"TemplateName\":\"EducationOfferList\"}]]" +
                                             Environment.NewLine;

                if (R<CategoryRepository>().GetChildren(category.Id)
                    .Any(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Media))
                    category.TopicMarkdown = category.TopicMarkdown + "[[{\"TemplateName\":\"MediaList\"}]]" +
                                             Environment.NewLine;

                category.TopicMarkdown = category.TopicMarkdown +
                                         "[[{\"TemplateName\":\"ContentLists\"}]]" + Environment.NewLine +
                                         "[[{\"TemplateName\":\"RelatedContentLists\"}]]" + Environment.NewLine +
                                         "[[{\"TemplateName\":\"CategoryNetwork\"}]]" + Environment.NewLine;

                Sl.CategoryRepo.Update(category);

                Logg.r().Information("{categoryId} {beforeMarkdown} {afterMarkdown}", categoryId,
                    topicMarkdownBeforeUpdate, category.TopicMarkdown);
            }
        }

        return View("Maintenance",
            new MaintenanceModel {Message = new SuccessMessage("Default Templates wurden migriert")});
    }

    [HttpPost]
    public ActionResult MigrateDescriptionToTemplates()
    {
        TemplateMigration.DescriptionMigration.Start();

        return View("Maintenance",
            new MaintenanceModel {Message = new SuccessMessage("Die Category Description wurden migriert")});
    }


    [HttpPost]
    public ActionResult UserDelete(ToolsModel toolsModel)
    {
        var searchTerm = toolsModel.UserId;
        Sl.UserRepo.DeleteFromAllTables(toolsModel.UserId);

        return View("Tools",
            new ToolsModel {Message = new SuccessMessage("Der User wurde gelöscht")});
    }
}

