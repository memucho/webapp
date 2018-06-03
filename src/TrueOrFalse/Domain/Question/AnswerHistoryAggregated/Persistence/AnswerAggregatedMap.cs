using System;
using FluentNHibernate.Mapping;

public class AnswerAggregatedMap : ClassMap<AnswerAggregated>
{
    public AnswerAggregatedMap()
    {
        Table("answer_aggregated");

        Id(x => x.Id);

        Map(x => x.QuestionId);
        Map(x => x.TotalTrue);
        Map(x => x.TotalFalse);
        Map(x => x.UserId);
        Map(x => x.LastUpdated);
    }           
}
