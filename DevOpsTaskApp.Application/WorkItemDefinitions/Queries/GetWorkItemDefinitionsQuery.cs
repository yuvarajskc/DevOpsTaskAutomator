using MediatR;
using System.Collections.Generic;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Queries;

public class GetWorkItemDefinitionsQuery : IRequest<List<WorkItemDefinitionDto>>
{
    public string? UserStoryId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
