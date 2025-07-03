using DevOpsTaskApp.Application.Common.Interfaces;
using DevOpsTaskApp.Domain.Entities;
using MediatR;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands;

public class CreateWorkItemDefinitionCommandHandler : IRequestHandler<CreateWorkItemDefinitionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateWorkItemDefinitionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateWorkItemDefinitionCommand request, CancellationToken cancellationToken)
    {
        var entity = new WorkItemDefinition
        {
            Title = request.Title,
            Description = request.Description,
            UserStoryId = request.UserStoryId,
            AssignedTo = request.AssignedTo,
            CreatedAt = DateTime.UtcNow
        };

        _context.WorkItemDefinitions.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
