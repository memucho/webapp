﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;

public class KnowledgeSummaryLoader
{
    public static KnowledgeSummary Run(
        int userId, 
        IEnumerable<int> questionIds = null, 
        bool onlyValuated = true)
    {
        var queryOver =
            Sl.R<ISession>() 
                .QueryOver<QuestionValuation>()
                .Select(
                    Projections.Group<QuestionValuation>(x => x.KnowledgeStatus),
                    Projections.Count<QuestionValuation>(x => x.KnowledgeStatus)
                )
                .Where(x => x.User.Id == userId);

        if (onlyValuated)
            queryOver.And(x => x.RelevancePersonal != -1);

        if (questionIds != null)
            queryOver.AndRestrictionOn(x => x.Question.Id)
                     .IsIn(questionIds.ToArray());

        var queryResult = queryOver.List<object[]>();

        var result = new KnowledgeSummary();

        foreach (var line in queryResult)
        {
            if ((int) line[0] == (int)KnowledgeStatus.NotLearned)
                result.NotLearned = (int)line[1];

            else if ((int) line[0] == (int)KnowledgeStatus.NeedsLearning)
                result.NeedsLearning = (int)line[1];

            else if ((int)line[0] == (int)KnowledgeStatus.NeedsConsolidation)
                result.NeedsConsolidation = (int)line[1];

            else if ((int)line[0] == (int)KnowledgeStatus.Solid)
                result.Solid = (int)line[1];
        }

        return result;
    }

    public static KnowledgeSummary Run(
        IList<TrainingQuestion> trainingQuestions, 
        bool beforeTraining = false, 
        bool afterTraining = false)
    {
        if(beforeTraining && afterTraining)
            throw new Exception();

        if (!beforeTraining && !afterTraining)
            throw new Exception();

        var result = new KnowledgeSummary();

        foreach (var question in trainingQuestions)
        {
            var value = beforeTraining 
                ? question.ProbBefore 
                : question.ProbAfter;

            if (value < 70)
                result.NeedsLearning += 1;
            else if (value < 90)
                result.NeedsConsolidation += 1;
            else 
                result.Solid += 1;
        }

        return result;
    }
}

