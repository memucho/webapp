using System;
using TrueOrFalse.Core;

namespace TrueOrFalse.Core
{
    public class QuestionServiceDemoData : IQuestionService
    {
        public void Create(Question question)
        {
            question.Id = QuestionDemoData.All().Count + 1;
            QuestionDemoData.All().Add(question);
        }

        public Question GetById(int id)
        {
            return QuestionDemoData.All().GetById(id);
        }

        public QuestionList GetAll()
        {
            return QuestionDemoData.All();
        }

        
    }
}