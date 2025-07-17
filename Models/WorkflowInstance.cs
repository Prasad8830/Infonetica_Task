namespace InfoneticaTask.Models;

public record WorkflowInstance
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string WorkflowId { get; init; } = string.Empty;
    public string CurrentState { get; set; } = string.Empty;
    public List<HistoryEntry> History { get; init; } = new();
}
