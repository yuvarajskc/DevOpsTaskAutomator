using System;
using MediatR;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands.DeleteWorkItemDefinition;

public class DeleteWorkItemDefinitionCommand : IRequest<Unit>
{
    public int Id { get; set; }

    public DeleteWorkItemDefinitionCommand(int id)
    {
        Id = id;
    }
}

