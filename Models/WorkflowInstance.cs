using System;
using System.Collections.Generic;
public class WorkflowInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string WorkflowId { get; set; } = default!;
    public string CurrentState { get; set; } = default!;
   public List<HistoryEntry> History { get; set; } = new();

}
