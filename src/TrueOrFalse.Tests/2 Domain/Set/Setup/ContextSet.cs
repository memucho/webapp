﻿using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

public class ContextSet
{
    private readonly ContextQuestion _contextQuestion = ContextQuestion.New();
    public readonly User Creator;
    private readonly ContextCategory _contextCategory = ContextCategory.New();
    private readonly SetRepo _setRepo;
    private readonly QuestionInSetRepo _questionInSetRepo;

    public List<Set> All = new List<Set>();
        
    private ContextSet()
    {
        Creator = ContextUser.GetUser();
        _setRepo = Sl.R<SetRepo>();
        _questionInSetRepo = Sl.R<QuestionInSetRepo>();
    }

    public static ContextSet New()
    {
        return new ContextSet();
    }

    public ContextSet AddSet(
        string name, 
        string text = "", 
        User creator = null, 
        int numberOfQuestions = 0, 
        IList<Question> questions = null)
    {
        var set = new Set{
            Name = name, 
            Text = text, 
            Creator =  creator ?? Creator
        };
            
        All.Add(set);

        if (questions != null)
            AddQuestions(questions);

        for (var i = 0; i < numberOfQuestions; i++)
            AddQuestion("question_" + i, "answer_" + i);

        return this;
    }

    public ContextSet AddCategory(string name)
    {
        var category = _contextCategory.Add(name).Persist().All.Last();
        _contextCategory.Persist();
            
        All.Last().Categories.Add(category);
        return this;
    }

    public ContextSet AddQuestions(IList<Question> questions)
    {
        foreach (var question in questions)
            AddQuestion(question);

        return this;
    }

    public ContextSet AddQuestion(string question, string solution)
    {
        _contextQuestion.AddQuestion(questionText: question, solutionText: solution).Persist();
        var addedQuestion = _contextQuestion.All.Last();

        return AddQuestion(addedQuestion);
    }

    public ContextSet AddQuestion(Question question)
    {
        var set = All.Last();
        this.Persist();

        var newQuestionInSet = new QuestionInSet
        {
            Question = question,
            Set = set
        };
        _questionInSetRepo.Create(newQuestionInSet);
        _questionInSetRepo.Flush();

        set.QuestionsInSet.Add(newQuestionInSet);
            

        return this;
    }

    public ContextSet Persist()
    {
        foreach (var set in All)
            _setRepo.Create(set);

        _setRepo.Flush();

        return this;
    }
}