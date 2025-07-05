using MediatR;
using System.Collections.Generic;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Queries;

public class GetWorkItemDefinitionsQuery : IRequest<List<WorkItemDefinitionDto>>
{
    public string? UserStoryId { get; set; }
}
