using System;
using MediatR;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands;

public class CreateWorkItemDefinitionCommand : IRequest<int>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string UserStoryId { get; set; } = default!;
    public string AssignedTo { get; set; } = default!;
}
