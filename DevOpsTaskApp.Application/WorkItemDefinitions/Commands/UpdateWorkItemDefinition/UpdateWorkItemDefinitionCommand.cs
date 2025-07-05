using System;
using MediatR;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands.UpdateWorkItemDefinition;

public class UpdateWorkItemDefinitionCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string UserStoryId { get; set; } = default!;
    public string AssignedTo { get; set; } = default!;
}
