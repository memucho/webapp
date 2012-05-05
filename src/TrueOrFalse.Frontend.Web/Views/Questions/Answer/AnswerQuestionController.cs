﻿using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

[HandleError]
public class AnswerQuestionController : Controller
{
    private readonly QuestionRepository _questionRepository;
    private readonly AnswerQuestion _answerQuestion;
    private readonly SessionUser _sessionUser;
    private readonly SessionUiData _sessionUiData;

    private const string _viewLocation = "~/Views/Questions/Answer/AnswerQuestion.aspx";

    public AnswerQuestionController(QuestionRepository questionRepository, 
                                    AnswerQuestion answerQuestion,
                                    SessionUser sessionUser, 
                                    SessionUiData sessionUiData)
    {
        _questionRepository = questionRepository;
        _answerQuestion = answerQuestion;
        _sessionUser = sessionUser;
        _sessionUiData = sessionUiData;
    }

    public ActionResult Answer(string text, int id, int elementOnPage)
    {
        var question = _questiSonRepository.GetById(id);

        return View(_viewLocation, new AnswerQuestionModel(question, _sessionUiData.QuestionSearchSpec));
    }

    public ActionResult Next()
    {
        _sessionUiData.QuestionSearchSpec.NextPage(1);
        return GetViewByCurrentSearchSpec();
    }

    public ActionResult Previous()
    {
        _sessionUiData.QuestionSearchSpec.PreviousPage(1);
        return GetViewByCurrentSearchSpec();
    }

    private ActionResult GetViewByCurrentSearchSpec()
    {
        var question = _questionRepository.GetBy(_sessionUiData.QuestionSearchSpec).Single();
        return View(_viewLocation, new AnswerQuestionModel(question, _sessionUiData.QuestionSearchSpec));
    }

    [HttpPost]
    public JsonResult SendAnswer(int id, string answer)
    {
        var result = _answerQuestion.Run(id, answer, _sessionUser.User.Id);

        return new JsonResult {Data = new
                                          {
                                              correct = result.IsCorrect, 
                                              correctAnswer = result.CorrectAnswer
                                          }};
    }

    [HttpPost]
    public JsonResult GetAnswer(int id, string answer)
    {
        var question = _questionRepository.GetById(id);
        return new JsonResult { Data = new { correctAnswer = question.Solution} };        
    }

}

