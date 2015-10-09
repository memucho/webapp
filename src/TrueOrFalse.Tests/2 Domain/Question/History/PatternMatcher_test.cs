﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class PatternMatcher_test 
{
    [Test]
    public void Should_match_x_days_3()
    {
        //ARRANGE
        var listOfAnswers = new List<AnswerHistory>()
        {
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 3, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 4, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 5, 14, 00, 21)},
        };

        //ACT
        var matches = AnswerPatternMatcher.Run(listOfAnswers);

        //ASSERT
        Assert.That(matches.Count, Is.EqualTo(1));
        Assert.That(matches[0].Name, Is.EqualTo("X-Days-3"));
    }

    [Test]
    public void Should_match_x_days_4()
    {
        //ARRANGE
        var listOfAnswers = new List<AnswerHistory>()
        {
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 3, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 4, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 5, 14, 00, 21)},
            new AnswerHistory {DateCreated = new DateTime(2015, 12, 6, 14, 00, 21)},
        };

        //ACT
        var matches = AnswerPatternMatcher.Run(listOfAnswers);

        //ASSERT
        Assert.That(matches.Count, Is.EqualTo(1));
        Assert.That(matches[0].Name, Is.EqualTo("X-Days-4"));
    }
}



public class AnswerPatternMatch
{
    public string Name;
}

public class AnswerPatternMatcher
{
    public static List<AnswerPatternMatch> Run(List<AnswerHistory> listOfAnswers)
    {
        var result = new List<AnswerPatternMatch>();


        AnswerPatternRepo.GetAll().ForEach(p => {
            if(p.IsMatch(listOfAnswers))
                result.Add(new AnswerPatternMatch { Name = p.Name });
        });

        return result;
    }
}

public class AnswerPatternRepo
{
    public static List<IAnswerPattern> GetAll()
    {
        return new List<IAnswerPattern>()
        {
            new XDays3(),
            new XDays4(),
        };
    }
}

public interface IAnswerPattern
{
    string Name { get; }
    bool IsMatch(List<AnswerHistory> listOfAnswers);
}

public class XDays
{
    public bool IsMatch(List<AnswerHistory> listOfAnswers, int days)
    {
        if (listOfAnswers.Count != days)
            return false;

        var listOfAnswersOrderedByDate = listOfAnswers.OrderBy(x => x.DateCreated).ToList();

        DateTime previousDate = DateTime.MinValue;
        for (var i = 0; i < listOfAnswersOrderedByDate.Count(); i++)
        {
            if (i == 0)
            {
                previousDate = listOfAnswersOrderedByDate[i].DateCreated;
                continue;
            }

            if (previousDate.AddDays(1).Date != listOfAnswersOrderedByDate[i].DateCreated.Date)
                return false;

            previousDate = listOfAnswersOrderedByDate[i].DateCreated;
        }

        return true;
    }
}

public class XDays3 : XDays, IAnswerPattern
{
    public string Name {get { return "X-Days-3"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 3);
    }
}

public class XDays4 : XDays, IAnswerPattern
{
    public string Name { get { return "X-Days-4"; } }

    public bool IsMatch(List<AnswerHistory> listOfAnswers)
    {
        return IsMatch(listOfAnswers, 4);
    }
}