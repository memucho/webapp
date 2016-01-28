﻿using FluentNHibernate.Mapping;

public class TrainingDateMap : ClassMap<TrainingDate>
{
    public TrainingDateMap()
    {
        Id(x => x.Id);

        HasOne(x => x.TrainingPlan).Cascade.None();

        Map(x => x.DateTime);

        HasMany(x => x.AllQuestions)
            .Table("trainingdate_questions")
            .Cascade.AllDeleteOrphan()
            .Not.KeyNullable();

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}