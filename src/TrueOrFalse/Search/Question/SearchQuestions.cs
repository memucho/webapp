using System;
using System.Collections.Generic;
using NHibernate.Util;
using Seedworks.Lib.Persistence;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace TrueOrFalse.Search
{
    public class SearchQuestions : IRegisterAsInstancePerLifetime
    {
        private readonly ISolrOperations<QuestionSolrMap> _searchOperations;

        public SearchQuestions(ISolrOperations<QuestionSolrMap> searchOperations){
            _searchOperations = searchOperations;
        }

        public SearchQuestionsResult Run(QuestionSearchSpec searchSpec)
        {
            var orderBy = SearchQuestionsOrderBy.None;
            if (searchSpec.OrderBy.BestMatch.IsCurrent()) orderBy = SearchQuestionsOrderBy.BestMatch;
            else if (searchSpec.OrderBy.OrderByQuality.IsCurrent()) orderBy = SearchQuestionsOrderBy.Quality;
            else if (searchSpec.OrderBy.Views.IsCurrent()) orderBy = SearchQuestionsOrderBy.Views;
            else if (searchSpec.OrderBy.CreationDate.IsCurrent()) orderBy = SearchQuestionsOrderBy.DateCreated;
            else if (searchSpec.OrderBy.PersonalRelevance.IsCurrent()) orderBy = SearchQuestionsOrderBy.Valuation;

            var result = Run(
                searchSpec.Filter.SearchTerm,
                searchSpec,
                searchSpec.Filter.CreatorId,
                searchSpec.Filter.IgnorePrivates,
                searchSpec.Filter.Categories,
                searchSpec.Filter.GetKnowledgeQuestionIds(),
                searchSpec.Filter.QuestionIdsToExclude,
                orderBy: orderBy
            );

            searchSpec.SpellCheck = new SpellCheckResult(result.SpellChecking, searchSpec.Filter.SearchTerm);

            return result;
        }

        public SearchQuestionsResult Run(string searchTerm){
            return Run(searchTerm, new Pager());
        }

        public SearchQuestionsResult Run(
            string searchTerm, 
            Pager pager,
            int creatorId = -1,
            bool ignorePrivates = true,
            IList<int> categories = null,
            IList<int> questionIds = null, 
            IList<int> questionIdsToExclude = null,
            SearchQuestionsOrderBy orderBy = SearchQuestionsOrderBy.None)
        {
            var sqb = new SearchQueryBuilder();

            var creatorFilter = QuestionFilter.GetCreatorFilterValue(searchTerm);
            if (creatorFilter != null)
            {
                var creator = Sl.Resolve<UserRepo>().GetByName(creatorFilter);
                if (creator != null)
                    creatorId = creator.Id;

                searchTerm = searchTerm.Replace("Ersteller:\"" + creatorFilter + "\"", "");
            }

            var categoryFilter = QuestionFilter.GetCategoryFilterValue(searchTerm);
            if (categoryFilter != null)
            {
                sqb.Add("Categories", categoryFilter, isAndCondition: true, exact: true);
                searchTerm = searchTerm.Replace("Kat:\"" + categoryFilter + "\"","");
            }
            else
                sqb.Add("Categories", searchTerm);


            sqb.Add("FullTextStemmed", searchTerm)
                .Add("FullTextExact", searchTerm)
                .Add("FullTextExact", searchTerm, startsWith: true)
                .Add("Text", searchTerm, boost: 1000)
                .Add("Text", searchTerm, startsWith: true, boost: 99999);
            //.Add("FullTextExact", searchTerm, phrase: true);

            sqb.Add("CreatorId", creatorId != -1 ? creatorId.ToString() : null, isAndCondition: true, exact: true);

            categories?
                .ForEach(x => sqb.Add("CategoryIds", x.ToString(), isAndCondition: true, exact: true));

            questionIds?
                .ForEach(x => sqb.Add("Id", x.ToString(), exact: true, isAndCondition:false));

            questionIdsToExclude?
                .ForEach(x => sqb.Add("Id", x.ToString(), exact: true, isNotCondition:true));

            if (ignorePrivates)
                sqb.Add("IsPrivate", "false", exact:true, isAndCondition:true);

            var orderby = new List<SortOrder>();
            if (orderBy == SearchQuestionsOrderBy.Quality)
                orderby.Add(new SortOrder("Quality", Order.DESC));
            else if(orderBy == SearchQuestionsOrderBy.Views)
                orderby.Add(new SortOrder("Views", Order.DESC));
            else if(orderBy == SearchQuestionsOrderBy.Valuation)
                orderby.Add(new SortOrder("Valuation", Order.DESC));
            else if (orderBy == SearchQuestionsOrderBy.DateCreated)
                orderby.Add(new SortOrder("DateCreated", Order.DESC));

            if(orderBy != SearchQuestionsOrderBy.BestMatch && orderBy != SearchQuestionsOrderBy.DateCreated)
                orderby.Add(new SortOrder("DateCreated", Order.DESC));

	        if (orderBy == SearchQuestionsOrderBy.BestMatch)
	        {
                if(String.IsNullOrEmpty(searchTerm))
		        {
                    orderby.Add(new SortOrder("Valuation", Order.DESC));
			        orderby.Add(new SortOrder("DateCreated", Order.DESC));
		        }
            }

            #if DEBUG
                Logg.r().Information("SearchQuestions {Query}", sqb.ToString());
            #endif
            
            var queryResult = _searchOperations.Query(sqb.ToString(),
                new QueryOptions
                {
                    Start = pager.LowerBound - 1,
                    Rows = pager.PageSize,
                    ExtraParams = new Dictionary<string, string> { { "qt", "dismax" } },
                    SpellCheck = new SpellCheckingParameters(),
                    OrderBy = orderby
                });

            var result = new SearchQuestionsResult();
            result.QueryTime = queryResult.Header.QTime;
            result.Count = queryResult.NumFound;
            result.SpellChecking = queryResult.SpellChecking;
            result.Pager = pager;
            
            pager.TotalItems = result.Count;

            foreach (var resultItem in queryResult)
                result.QuestionIds.Add(resultItem.Id);

            return result;
        }
    }
}