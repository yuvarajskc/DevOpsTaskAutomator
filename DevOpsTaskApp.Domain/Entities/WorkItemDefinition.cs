namespace DevOpsTaskApp.Domain.Entities;

public class WorkItemDefinition
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string UserStoryId { get; set; } = default!;
    public string AssignedTo { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
