using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Seedworks.Lib.Persistence;
using static System.String;
using SpellCheckResult = TrueOrFalse.Search.SpellCheckResult;

[Serializable]
public class QuestionSearchSpec : SearchSpecificationBase<QuestionFilter, QuestionOrderBy>
{
    public QuestionSearchSpec(bool ignorePrivates = true)
    {
        Filter.IgnorePrivates = ignorePrivates;
    }

    public string Key;
    public SearchTabType SearchTab;
    public SpellCheckResult SpellCheck;

    public QuestionHistoryItem HistoryItem;

    public string GetSuggestion()
    {
        if (SpellCheck == null)
            return "";

        return SpellCheck.GetSuggestion();
    }
}

[Serializable]
public class QuestionFilter : ConditionContainer
{
    public string SearchTerm;
    public int CreatorId = -1;
    public bool IgnorePrivates = true;

    public IList<int> Categories = new List<int>();
    public IList<int> QuestionIdsToExclude = new List<int>();

    public bool Knowledge_Solid = true;
    public bool Knowledge_ShouldConsolidate = true;
    public bool Knowledge_ShouldLearn = true;
    public bool Knowledge_None = true;

    public bool Knowledge_FilterIsSet => 
        !(Knowledge_Solid && Knowledge_ShouldConsolidate &&
        Knowledge_ShouldLearn && Knowledge_None);

    public bool Knowledge_FilterAllFalse =>
        !Knowledge_Solid && !Knowledge_ShouldConsolidate &&
        !Knowledge_ShouldLearn && !Knowledge_None;

    public IList<int> GetKnowledgeQuestionIds()
    {
        if(!Knowledge_FilterIsSet)
            return null;

        return Sl.R<QuestionRepo>()
            .GetByKnowledge(
                Sl.CurrentUserId,
                isKnowledgeSolidFilter: Knowledge_Solid,
                isKnowledgeShouldConsolidateFilter: Knowledge_ShouldConsolidate,
                isKnowledgeShouldLearnFilter: Knowledge_ShouldLearn,
                isKnowledgeNoneFilter: Knowledge_None);
    } 

    public static string GetCategoryFilterValue(string searchTerm)
    {
        return GetFilter("Kat", searchTerm);
    }

    public static string GetCreatorFilterValue(string searchTerm)
    {
        return GetFilter("Ersteller", searchTerm);
    }

    private static string GetFilter(string key, string searchTerm)
    {
        string filter = null;
        if (searchTerm != null && searchTerm.IndexOf(key + ":\"") != -1)
        {
            var match = Regex.Match(searchTerm, key + ":\"(.*)\"", RegexOptions.IgnoreCase);
            if (match.Success)
                filter = match.Groups[1].Value;
        }
        return filter;
    }

    public new void Clear()
    {
        SearchTerm = "";
        CreatorId = -1;
        IgnorePrivates = true;
        Categories.Clear();

        base.Clear();
    }

    public bool HasExactOneCategoryFilter() => Categories.Count == 1 && IsNullOrEmpty(SearchTerm);

    public void SetKnowledgeFilter(string filter)
    {
        UnsetAllKnowledgeFilters();

        switch (filter)
        {
            case "solid": Knowledge_Solid = true; return;
            case "consolidate": Knowledge_ShouldConsolidate = true; return;
            case "learn": Knowledge_ShouldLearn = true; return;
            case "notLearned": Knowledge_None = true; return;
        }
    }

    public void UnsetAllKnowledgeFilters()
    {
        this.Knowledge_Solid = false;
        this.Knowledge_ShouldConsolidate = false;
        this.Knowledge_ShouldLearn = false;
        this.Knowledge_None = false;
    }
}

[Serializable]
public class QuestionOrderBy : SpecOrderByBase
{
    public OrderBy BestMatch;
    public OrderBy PersonalRelevance;
    public OrderBy OrderByQuality;
    public OrderBy Views;
    public OrderBy CreationDate;

    public QuestionOrderBy()
    {
        BestMatch = new OrderBy("BestMatch", this);
        PersonalRelevance = new OrderBy("TotalRelevancePersonalAvg", this);
        OrderByQuality = new OrderBy("TotalQualityAvg", this);
        Views = new OrderBy("TotalViews", this);
        CreationDate = new OrderBy("DateCreated", this);
    }

    public string ToText()
    {
        if (BestMatch.IsCurrent())
            return "Beste Treffer";

        if (PersonalRelevance.IsCurrent())
            return "Gemerkt";

        if (OrderByQuality.IsCurrent())
            return "Qualität";

        if (Views.IsCurrent())
            return "Gesehen";

        if (CreationDate.IsCurrent())
            return "Datum erstellt";

        return "";
    }
}