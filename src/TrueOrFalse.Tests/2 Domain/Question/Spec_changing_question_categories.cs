using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class Spec_changing_question_categories : BaseTest
    {
        [Test]
        public void Should_change_categories()
        {
            var questionContext =
                ContextQuestion.New().AddQuestion(questionText: "Question", solutionText: "Answer")
                                       .AddCategory("A")
                                       .AddCategory("B")
                                       .AddCategory("C")
                                     .Persist();

            var question = questionContext.All[0];
            var categoryA = question.Categories.ByName("A");
            var categoryB = question.Categories.ByName("B");

            question.Categories.Clear();
            question.Categories.Add(categoryA);
            question.Categories.Add(categoryB);

            var categoryRepository = Resolve<QuestionRepo>();
            categoryRepository.Update(question);

            var questionFromDb = categoryRepository.GetAll()[0];
            Assert.That(questionFromDb.Categories.Count, Is.EqualTo(2));
            Assert.That(questionFromDb.Categories[0].Name, Is.EqualTo("A"));
            Assert.That(questionFromDb.Categories[1].Name, Is.EqualTo("B"));
        }

        [Test]
        [Ignore("Temporarily")]
        public void Should_update_affected_categories()
        {
            ContextCategory.New().Add("1").Add("2").Add("3").Persist();

            var category1 = Sl.R<CategoryRepository>().GetByName("1").FirstOrDefault();
            var category2 = Sl.R<CategoryRepository>().GetByName("2").FirstOrDefault();
            var category3 = Sl.R<CategoryRepository>().GetByName("3").FirstOrDefault();

            var questionContext =
                ContextQuestion.New()
                    .AddQuestion(questionText: "Question1", solutionText: "Answer", categories: new List<Category>{category1})
                    .AddQuestion(questionText: "Question2", solutionText: "Answer", categories: new List<Category> { category2 })
                    .AddQuestion(questionText: "Question3", solutionText: "Answer", categories: new List<Category> { category3 })
                    .Persist();

            var question2Id = questionContext.All.First(q => q.Text== "Question2").Id;
            var question3Id = questionContext.All.First(q => q.Text== "Question3").Id;

            Assert.That(category1.CountQuestions, Is.EqualTo(1));
            Assert.That(category2.CountQuestions, Is.EqualTo(1));
            Assert.That(category3.CountQuestions, Is.EqualTo(1));


            ModifyRelationsForCategory.AddParentCategory(category2, category1);
            ModifyRelationsForCategory.AddParentCategory(category3, category2);


            RecycleContainer();//Relations need to be persisted to be considered by ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf()

            category1 = Sl.R<CategoryRepository>().GetByName("1").FirstOrDefault();
            category2 = Sl.R<CategoryRepository>().GetByName("2").FirstOrDefault();
            category3 = Sl.R<CategoryRepository>().GetByName("3").FirstOrDefault();


            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category1);
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category2);
            ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category3);

            Assert.That(category1.CountQuestions, Is.EqualTo(1));
            Assert.That(Sl.QuestionRepo.GetForCategory(category1.Id).Count, Is.EqualTo(1));
            Assert.That(category1.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(3));
            Assert.That(category1.GetAggregatedContentFromJson().AggregatedQuestions.Any(q => q.Text == "Question3"));

            Assert.That(category2.CountQuestions, Is.EqualTo(1));
            Assert.That(Sl.QuestionRepo.GetForCategory(category2.Id).Count, Is.EqualTo(1));
            Assert.That(category2.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(2));
            Assert.That(category2.GetAggregatedContentFromJson().AggregatedQuestions.Any(q => q.Text == "Question3"));

            Assert.That(category3.CountQuestions, Is.EqualTo(1));
            Assert.That(Sl.QuestionRepo.GetForCategory(category3.Id).Count, Is.EqualTo(1));
            Assert.That(category3.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(1));
            Assert.That(category3.GetAggregatedContentFromJson().AggregatedQuestions.Any(q => q.Text == "Question3"));


            var question3 = Sl.R<QuestionRepo>().GetById(question3Id);

            question3.Categories.Remove(category3);

            Sl.QuestionRepo.Update(question3);

            Assert.That(Sl.QuestionRepo.GetForCategory(category1.Id).Count, Is.EqualTo(1));
            Assert.That(category1.CountQuestions, Is.EqualTo(1));
            Assert.That(category1.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(2));
            Assert.That(category1.GetAggregatedContentFromJson().AggregatedQuestions.All(q => q.Id != question3Id));

            Assert.That(Sl.QuestionRepo.GetForCategory(category2.Id).Count, Is.EqualTo(1));
            Assert.That(category2.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(1));
            Assert.That(category2.GetAggregatedContentFromJson().AggregatedQuestions.All(q => q.Id != question3Id));

            Assert.That(Sl.QuestionRepo.GetForCategory(category3.Id).Count, Is.EqualTo(0));
            Assert.That(category3.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(0));


            QuestionDelete.Run(question2Id);

            Assert.That(Sl.QuestionRepo.GetForCategory(category1.Id).Count, Is.EqualTo(1));
            Assert.That(Sl.QuestionRepo.GetForCategory(category2.Id).Count, Is.EqualTo(0));

            Assert.That(category1.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(1));
            Assert.That(category1.CountQuestionsAggregated, Is.EqualTo(1));
            Assert.That(category1.CountQuestions, Is.EqualTo(1));

            Assert.That(category2.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(0));
            Assert.That(category2.CountQuestionsAggregated, Is.EqualTo(0));
            Assert.That(category2.CountQuestions, Is.EqualTo(0));


            question3.Categories.Add(category3);

            Sl.QuestionRepo.Update(question3);

            Assert.That(category1.CountQuestions, Is.EqualTo(1));
            Assert.That(Sl.QuestionRepo.GetForCategory(category1.Id).Count, Is.EqualTo(1));
            Assert.That(category1.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(2));
            Assert.That(category1.GetAggregatedContentFromJson().AggregatedQuestions.Any(q => q.Text == "Question3"));

            Assert.That(category2.CountQuestions, Is.EqualTo(0));
            Assert.That(Sl.QuestionRepo.GetForCategory(category2.Id).Count, Is.EqualTo(0));
            Assert.That(category2.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(1));
            Assert.That(category2.GetAggregatedContentFromJson().AggregatedQuestions.Any(q => q.Text == "Question3"));

            Assert.That(category3.CountQuestions, Is.EqualTo(1));
            Assert.That(Sl.QuestionRepo.GetForCategory(category3.Id).Count, Is.EqualTo(1));
            Assert.That(category3.GetAggregatedContentFromJson().AggregatedQuestions.Count, Is.EqualTo(1));
            Assert.That(category3.GetAggregatedContentFromJson().AggregatedQuestions.Any(q => q.Text == "Question3"));

        }
    }
}