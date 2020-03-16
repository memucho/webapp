using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;
using static System.String;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class Set : DomainEntity, ICreator
{
    public virtual string Name { get; set; }
    public virtual string Text { get; set; }
    public virtual string VideoUrl { get; set; }
    public virtual string VideoKey => YoutubeVideo.GetVideoKeyFromUrl(VideoUrl);

    public virtual ISet<QuestionInSet> QuestionsInSet{ get; set;}

    public virtual IList<QuestionInSet> QuestionsInSetPublic => 
        QuestionsInSet.Where(qs => qs.Question.IsVisibleToCurrentUser()).ToList();
        

    public virtual User Creator { get; set; }

    public virtual int TotalRelevancePersonalAvg { get; set; }
    public virtual int TotalRelevancePersonalEntries { get; set; }

    public virtual IList<Category> Categories { get; set; }

    public virtual Set CopiedFrom { get; set; }
    public virtual IList<Set> CopiedInstances { get; set; }

    public virtual void Add(Question question){
        QuestionsInSet.Add(
            new QuestionInSet{
                    Set = this,
                    Question = question,
                    Sort = QuestionsInSet.Count + 1
            }
        );
    }

    public virtual void Add(IList<Question> questions){
        foreach (var question in questions){
            Add(question);
        }
    }

    public Set(){
        QuestionsInSet = new HashSet<QuestionInSet>();
        Categories = new List<Category>();
    }

    public virtual IList<int> QuestionIds() => 
        QuestionsInSet
            .Select(qv => qv.Question.Id)
            .ToList();

    public virtual IList<Question> Questions() => 
        QuestionsInSet.Select(q => q.Question).ToList();

    public virtual IList<Question> QuestionsPublic() => 
        QuestionsInSetPublic.Select(qs => qs.Question).ToList();

    public virtual int QuestionCount() =>
        QuestionsInSet.Count;

    public virtual int QuestionsPublicCount() =>
        QuestionsInSetPublic.Count;

    public virtual bool HasVideo => 
        !IsNullOrEmpty(VideoUrl) && 
        !IsNullOrEmpty(YoutubeVideo.GetVideoKeyFromUrl(VideoUrl));
}