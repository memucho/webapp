﻿using System;
using System.Web;
using NHibernate;
using TrueOrFalse.Search;

public class SaveQuestionView : IRegisterAsInstancePerLifetime
{
    private readonly QuestionViewRepository _questionViewRepo;
    private readonly SearchIndexQuestion _searchIndexQuestion;
    private readonly ISession _session;

    public SaveQuestionView(
        QuestionViewRepository questionViewRepo, 
        SearchIndexQuestion searchIndexQuestion,
        ISession session)
    {
        _questionViewRepo = questionViewRepo;
        _searchIndexQuestion = searchIndexQuestion;
        _session = session;
    }

    public void Run(Guid questionViewGuid, Question question, User user)
    {
        Run(questionViewGuid, question, user == null ? -1 : user.Id);
    }

    public void Run(
        Guid questionViewGuid,
        Question question,
        int userId,
        Player player = null,
        Round round = null,
        LearningSession learningSession = null,
        Guid learningSessionStepGuid = default(Guid))
    {
        if (userId != -1) //if user is logged in, always log
            if (HttpContext.Current != null && HttpContext.Current.Request.Browser.Crawler)
                return;

        var questionView = _questionViewRepo.GetById(1231048);

        _questionViewRepo.Create(new QuestionView
        {
            Guid = questionViewGuid,
            QuestionId = question.Id,
            UserId = userId,
            Player = player,
            Round = round,
            LearningSession = learningSession,
            LearningSessionStepGuid = learningSessionStepGuid
        });
        _session.CreateSQLQuery("UPDATE Question SET TotalViews = " + _questionViewRepo.GetViewCount(question.Id) + " WHERE Id = " + question.Id).
            ExecuteUpdate();

        _searchIndexQuestion.Update(question);
    }
}
