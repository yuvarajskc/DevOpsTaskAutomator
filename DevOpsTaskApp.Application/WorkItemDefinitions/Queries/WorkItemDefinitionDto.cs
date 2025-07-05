namespace DevOpsTaskApp.Application.WorkItemDefinitions.Queries;

public class WorkItemDefinitionDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string UserStoryId { get; set; } = default!;
    public string AssignedTo { get; set; } = default!;
}
