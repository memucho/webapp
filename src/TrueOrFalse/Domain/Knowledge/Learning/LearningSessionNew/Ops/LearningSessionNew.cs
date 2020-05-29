﻿using System;
using System.Collections.Generic;

[Serializable]
public class LearningSessionNew
{
    public IList<LearningSessionStepNew> Steps;
    public LearningSessionConfig Config;
    public int Pager;

    public int CurrentIndex { get; private set; }
    public bool IsLastStep { get; private set; }
    public LearningSessionStepNew CurrentStep => Steps[CurrentIndex];
    public string UrlName = ""; 

    public User User;
    public bool IsLoggedIn;


    public LearningSessionNew(List<LearningSessionStepNew> learningSessionSteps, LearningSessionConfig config)
    {
        Steps = learningSessionSteps;
        var userCashItem = UserCache.GetItem(config.UserId);
        User = userCashItem.User;  
        IsLoggedIn =  config.UserId != -1;
        Config = config; 
    }

    public bool AddAnswer(AnswerQuestionResult answer)
    {
        CurrentStep.AnswerState = answer.IsCorrect ? AnswerStateNew.Correct : AnswerStateNew.False;
        if (Config.ReAddStepsToEnd() && !answer.IsCorrect)
        {
            ReAddCurrentStepToEnd();
            return true;
        }

        return false;
    }

    public void NextStep()
    {
        IsLastStep = TestIsLastStep();

        if (!IsLastStep)
            CurrentIndex++;
    }

    public void SkipStep()
    {
        CurrentStep.AnswerState = AnswerStateNew.Skipped;

        if (Config.ReAddStepsToEnd())
            ReAddCurrentStepToEnd();
 
        IsLastStep = TestIsLastStep();

        if(!IsLastStep)
            CurrentIndex++;
    }

    private void ReAddCurrentStepToEnd()
    {
        var step = new LearningSessionStepNew(CurrentStep.Question);
        Steps.Add(step);
    }

    private bool TestIsLastStep()
    {
        return CurrentIndex == Steps.Count - 1;
    }
}