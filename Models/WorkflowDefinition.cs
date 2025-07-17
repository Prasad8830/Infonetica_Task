using System;
using System.Collections.Generic;
public class WorkflowDefinition
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public List<State> States { get; set; } = new();
    public List<ActionTransition> Actions { get; set; } = new();
}
