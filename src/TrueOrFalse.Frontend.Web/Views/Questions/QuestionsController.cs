﻿using System;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using Seedworks.Lib;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

public class QuestionsController : Controller
{
    private readonly QuestionRepository _questionRepository;
    private readonly QuestionValuationRepository _questionValuationRepository;
    private readonly TotalsPersUserLoader _totalsPerUserLoader;
    private readonly UserRepository _userRepository;
    private readonly SessionUiData _sessionUiData;
    private readonly SessionUser _sessionUser;

    public QuestionsController (QuestionRepository questionRepository,
                                QuestionValuationRepository questionValuationRepository,
                                TotalsPersUserLoader totalsPerUserLoader, 
                                UserRepository userRepository, 
                                SessionUiData sessionUiData, 
                                SessionUser sessionUser)
    {
        _questionRepository = questionRepository;
        _questionValuationRepository = questionValuationRepository;
        _totalsPerUserLoader = totalsPerUserLoader;
        _userRepository = userRepository;
        _sessionUiData = sessionUiData;
        _sessionUser = sessionUser;
    }

    public ActionResult OrderByPersonalRelevance(int? page, QuestionsModel model){
        _sessionUiData.QuestionSearchSpec.OrderBy.OrderByPersonalRelevance.Desc();
        return Questions(page, model);
    }

    public ActionResult OrderByQuality(int? page, QuestionsModel model){
        _sessionUiData.QuestionSearchSpec.OrderBy.OrderByQuality.Desc();
        return Questions(page, model);
    }

    public ActionResult OrderByCreationDate(int? page, QuestionsModel model){
        _sessionUiData.QuestionSearchSpec.OrderBy.OrderByCreationDate.Desc();
        return Questions(page, model);
    }

    public ActionResult OrderByViews(int? page, QuestionsModel model){
        _sessionUiData.QuestionSearchSpec.OrderBy.OrderByViews.Desc();
        return Questions(page, model);
    }

    public ActionResult Questions(int? page, QuestionsModel model)
    {
        _sessionUiData.QuestionSearchSpec.PageSize = 10;

        _sessionUiData.QuestionSearchSpec.SetFilterByMe(model.FilterByMe);
        _sessionUiData.QuestionSearchSpec.SetFilterByAll(model.FilterByAll);
        _sessionUiData.QuestionSearchSpec.AddFilterByUser(model.AddFilterUser);
        _sessionUiData.QuestionSearchSpec.DelFilterByUser(model.DelFilterUser);

        if (!_sessionUiData.QuestionSearchSpec.OrderBy.IsSet())
            _sessionUiData.QuestionSearchSpec.OrderBy.OrderByPersonalRelevance.Desc();
        
        if (page.HasValue) _sessionUiData.QuestionSearchSpec.CurrentPage = page.Value;

        var session = ServiceLocator.Resolve<ISession>();
        session.CreateCriteria<Category>();
        
        var questions = _questionRepository.GetBy(_sessionUiData.QuestionSearchSpec, c => c.SetFetchMode("Categories", FetchMode.Eager));

        var totalsForCurrentUser = _totalsPerUserLoader.Run(_sessionUser.User.Id, questions);
        var questionValutionsForCurrentUser = _questionValuationRepository.GetBy(questions.GetIds(), _sessionUser.User.Id);

        return View("Questions",
                    new QuestionsModel(
                        questions, 
                        totalsForCurrentUser, 
                        questionValutionsForCurrentUser,
                        _sessionUiData.QuestionSearchSpec, 
                        _sessionUser.User.Id)
                    {
                        Pager = new PagerModel(_sessionUiData.QuestionSearchSpec),
                        FilterByMe = _sessionUiData.QuestionSearchSpec.FilterByMe,
                        FilterByAll = _sessionUiData.QuestionSearchSpec.FilterByAll,
                        FilterByUsers =  _userRepository.GetByIds(_sessionUiData.QuestionSearchSpec.FilterByUsers.ToArray()).ToDictionary(user => user.Id, user => user.Name),
                        TotalQuestionsInSystem = Sl.Resolve<GetTotalQuestionCount>().Run()
                    }
            );
    }

    [HttpPost]
    public JsonResult DeleteDetails(int questionId)
    {
        var question = _questionRepository.GetById(questionId);

        return new JsonResult{
            Data = new
                       {
                           questionTitle = question.Text.WordWrap(50),
                           totalAnswers = question.TotalAnswers()
                       }
        };
    }

    [HttpPost] 
    public EmptyResult Delete(int questionId)
    {
        Sl.Resolve<QuestionDeleter>().Run(questionId);
        return new EmptyResult();
    }

}
