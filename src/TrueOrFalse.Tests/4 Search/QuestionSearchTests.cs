using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NHibernate;
using NUnit.Framework;
using Seedworks.Lib.Persistence;
using SolrNet;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests
{
    public class QuestionSearchTests : BaseTest
    {
        [Test]
        public void Should_insert_and_query_document()
        {
            var solrQuestionMap = new QuestionSolrMap
                {
                    Id = 1,
                    DateCreated = DateTime.Now, 
                    CreatorId = 1,
                    Categories = {"Cat1", "Cat2"},
                    Text = "Title",
                    Description = "Description",
                    Solution = "Solution",
                    SolutionType = 2,
                    AvgQuality = 70,
                    Views = 23424
                };

            var solrOperations = Resolve<ISolrOperations<QuestionSolrMap>>();
            solrOperations.Delete(new SolrQuery("*:*"));
            solrOperations.Commit();

            solrOperations.Add(solrQuestionMap);
            solrOperations.Commit();

            var result = solrOperations.Query(new SolrQueryByField("FullTextExact", "Solution"));
            Assert.That(result.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void Should_reindex_all_questions()
        {
            Resolve<ISession>().Delete("FROM QuestionValuation");

            var context = ContextQuestion.New()
                .AddQuestion(questionText: "Question1", solutionText: "Answer2").AddCategory("A").Persist()
                .AddQuestion(questionText: "Question2", solutionText: "Answer3").AddCategory("B").Persist()
                .AddQuestion(questionText: "Juliane Misdom", solutionText: "Answer3").AddCategory("B").Persist();

            Resolve<ReIndexAllQuestions>().Run();

            Assert.That(Resolve<SearchQuestions>().Run("Juliane", new Pager()).Count, Is.EqualTo(1));
            Assert.That(Resolve<SearchQuestions>().Run("Juliane Misdom", new Pager()).QuestionIds.Count, Is.EqualTo(1)); ;
            Assert.That(Resolve<SearchQuestions>().Run("Question2", new Pager()).Count, Is.EqualTo(2)); ;

            Should_filter_by_creator_id(context);
        }

        public void Should_filter_by_creator_id(ContextQuestion contextQuestion)
        {
            Assert.That(Resolve<SearchQuestions>().Run(
                "", new Pager(), creatorId: contextQuestion.Creator.Id).Count, Is.EqualTo(3));
        }


        [Test]
        public void Should_filter_by_valuator_id()
        {
            InitializeContainer();
            Resolve<ReIndexAllQuestions>();

            var context = ContextQuestion.New()
                .AddQuestion(questionText: "Question1", solutionText: "Answer2").AddCategory("A")
                .AddQuestion(questionText: "Question2", solutionText: "Answer3").AddCategory("B")
                .AddQuestion(questionText: "Juliane Misdom", solutionText: "Answer3").AddCategory("B")
                .Persist();

            var user = context.Creator;

            Resolve<QuestionValuationRepo>().Create(new List<QuestionValuation>{
                new QuestionValuation { RelevancePersonal = 70, Question = context.All[0], User = user },
                new QuestionValuation { RelevancePersonal = 15, Question = context.All[1], User = user },
            });

            R<ISolrOperations<QuestionSolrMap>>().Commit();
        }

        [Test]
        public void Should_get_paged_result()
        {
            SessionFactory.TruncateAllTables();
            var context = ContextQuestion.New();
            Enumerable.Range(1, 50).ToList().ForEach(x => context.AddQuestion(questionText: "Question" + x, solutionText: "Answer" + x).AddCategory("Question"));
            context.Persist();

            Resolve<ReIndexAllQuestions>().Run();

            Assert.That(Resolve<SearchQuestions>().Run("\"Question35\"", new Pager { PageSize = 10 }).Count, Is.EqualTo(1));//Title
            Assert.That(Resolve<SearchQuestions>().Run("\"Answer35\"", new Pager { PageSize = 10 }).Count, Is.EqualTo(1));//Title

            var result = Resolve<SearchQuestions>().Run("Question", new Pager { PageSize = 10 });
            Assert.That(result.Count, Is.EqualTo(50));//Category
            Assert.That(result.QuestionIds.Count, Is.EqualTo(10));//Result is always paged
        }

        [Test]
        [Ignore("")]
        public void Should_order_search_result()
        {
            Resolve<ReIndexAllQuestions>().Run();

            var context = ContextQuestion.New()
                .AddQuestion(questionText: "Question1", solutionText: "Answer1").TotalQualityAvg(10).TotalValuationAvg(50)
                .AddQuestion(questionText: "Question2", solutionText: "Answer2").TotalQualityAvg(50).TotalValuationAvg(1)
                .AddQuestion(questionText: "Question3", solutionText: "Answer3").TotalValuationAvg(99)
                .Persist();

            R<ISolrOperations<QuestionSolrMap>>().Commit();

            var result = Resolve<SearchQuestions>().Run("Question", 
                new Pager { PageSize = 10 }, orderBy: SearchQuestionsOrderBy.Quality);
            var questions = Resolve<QuestionRepo>().GetByIds(result.QuestionIds);

            Assert.That(questions.Count, Is.EqualTo(3));
            Assert.That(questions[0].Text, Is.EqualTo("Question2"));
            Assert.That(questions[1].Text, Is.EqualTo("Question1"));
            Assert.That(questions[2].Text, Is.EqualTo("Question3"));

            result = Resolve<SearchQuestions>().Run("Question",
                new Pager { PageSize = 10 }, orderBy: SearchQuestionsOrderBy.Valuation);
            questions = Resolve<QuestionRepo>()
                .GetByIds(result.QuestionIds)
                .OrderBy(q => q.Text)
                .ToList();
            
            Assert.That(questions[0].Text, Is.EqualTo("Question1"));
            Assert.That(questions[1].Text, Is.EqualTo("Question2"));
            Assert.That(questions[2].Text, Is.EqualTo("Question3"));
        }

        [Test]
        public void Should_filter_by_category()
        {
            var context = ContextQuestion.New()
                .AddQuestion(questionText: "Question1", solutionText: "Answer2").AddCategory("C")
                .AddQuestion(questionText: "Question2", solutionText: "Answer3").AddCategory("B")
                .AddQuestion(questionText: "Juliane Misdom", solutionText: "Answer3").AddCategory("B")
                .Persist();

            R<ReIndexAllQuestions>().Run();

            Assert.That(Resolve<SearchQuestions>().Run("Kat:\"C\"", new Pager { PageSize = 10 }).QuestionIds.Count, Is.EqualTo(1));
            Assert.That(Resolve<SearchQuestions>().Run("Kat:\"B\"", new Pager { PageSize = 10 }).QuestionIds.Count, Is.EqualTo(2));
        }
    }
}