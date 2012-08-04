﻿using System.Linq;
using System.Collections.Generic;
using TrueOrFalse.Frontend.Web.Models;
using TrueOrFalse.Core;

public class QuestionsModel : ModelBase
{
    public QuestionsModel()
    {
        QuestionRows = Enumerable.Empty<QuestionRowModel>();
        FilterByUsers = new Dictionary<int, string>();
    }

    public QuestionsModel(IEnumerable<Question> questions, 
                          IEnumerable<TotalPerUser> totalsForCurrentUser, 
                          IEnumerable<QuestionValuation> questionValutionsForCurrentUser, 
                          QuestionSearchSpec questionSearchSpec, 
                          int currentUserId)
    {
        int counter = 0; 
        QuestionRows = from question in questions
                       select new QuestionRowModel(
                                    question,
                                    totalsForCurrentUser.ByQuestionId(question.Id),
                                    NotNull.Run(questionValutionsForCurrentUser.ByQuestionId(question.Id)),
                                    ((questionSearchSpec.CurrentPage - 1) * questionSearchSpec.PageSize) + ++counter, 
                                    currentUserId
                                  );

        FilterByUsers = new Dictionary<int, string>();
        TotalQuestionsInResult = questionSearchSpec.TotalItems;

        OrderBy = questionSearchSpec.OrderBy;
        OrderByLabel = questionSearchSpec.OrderBy.ToText();

        MenuLeftModel.Categories = questions.GetAllCategories()
                                    .GroupBy(c => c.Name)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g =>  new MenuModelCategoryItem{Category = g.First(), OnPageCount = g.Count()})
                                    .ToList();
    }

    public IEnumerable<QuestionRowModel> QuestionRows { get; set; }

    public PagerModel Pager { get; set; }

    public bool? FilterByMe { get; set; }
    public bool? FilterByAll { get; set; }
    public int? AddFilterUser { get; set; }
    public int? DelFilterUser { get; set; }
    public Dictionary<int, string> FilterByUsers { get; set; }

    public int TotalWishKnowledge;
    public int TotalQuestionsInResult;
    public int TotalQuestionsInSystem;
    public string OrderByLabel;
    public QuestionOrderBy OrderBy;
}