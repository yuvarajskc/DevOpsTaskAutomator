using System;
using MediatR;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands;

/// <summary>
/// Command to create a new work item definition.
/// </summary>
public class CreateWorkItemDefinitionCommand : IRequest<int>
{
    /// <summary>
    /// Gets or sets the title of the work item.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Gets or sets the description of the work item.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// Gets or sets the identifier of the related user story.
    /// </summary>
    public string UserStoryId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user to whom the work item is assigned.
    /// </summary>
    public string AssignedTo { get; set; } = default!;
}
