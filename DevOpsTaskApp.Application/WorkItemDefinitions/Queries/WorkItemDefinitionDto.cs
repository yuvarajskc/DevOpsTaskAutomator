namespace DevOpsTaskApp.Application.WorkItemDefinitions.Queries;

/// <summary>
/// Data Transfer Object representing a work item definition.
/// </summary>
public class WorkItemDefinitionDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the work item definition.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the work item.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Gets or sets the description of the work item.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user story identifier associated with the work item.
    /// </summary>
    public string UserStoryId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the user to whom the work item is assigned.
    /// </summary>
    public string AssignedTo { get; set; } = default!;
}
