using System;
using Seedworks.Lib.Persistence;

public class JobHistory : IPersistable
{
    public int Id { get; set; }
    public virtual JobHistoryType JobHistoryType { get; set; }

    public virtual DateTime FinishedAt { get; set; }
    public virtual DateTime StartedAt { get; set; }
}