namespace InfoneticaTask.Models;

public record ActionTransition(
    string Id,
    string Name,
    bool Enabled,
    List<string> FromStates,
    string ToState
);
