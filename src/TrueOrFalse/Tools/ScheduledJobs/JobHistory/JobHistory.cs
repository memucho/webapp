using System;
using Seedworks.Lib.Persistence;

public class JobHistory : IPersistable
{
    public virtual int Id { get; set; }
    public virtual JobHistoryType Type { get; set; }

    public virtual DateTime FinishedAt { get; set; }
    public virtual DateTime StartedAt { get; set; }
}