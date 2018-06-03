using System;
using Seedworks.Lib.Persistence;

public class AnswerAggregated : IPersistable
{
    public virtual int Id { get; set; }

    public virtual int QuestionId { get; set; }
    public virtual int TotalTrue { get; set; }
    public virtual int TotalFalse { get; set; }

    public virtual int UserId { get; set; }

    public virtual DateTime LastUpdated { get; set; }
}