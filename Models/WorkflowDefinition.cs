namespace InfoneticaTask.Models;

public record WorkflowDefinition(
    string Id,
    string Name,
    List<State> States,
    List<ActionTransition> Actions
);
