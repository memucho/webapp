using System;
using Seedworks.Lib.Persistence;

public class JobQueue : Entity
{
    public virtual JobQueueType JobQueueType { get; set; }
    public virtual string JobContent { get; set; }
}