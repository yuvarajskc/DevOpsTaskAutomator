using DevOpsTaskApp.Application.Common.Interfaces;
using DevOpsTaskApp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands;

public class CreateWorkItemDefinitionCommandHandler : IRequestHandler<CreateWorkItemDefinitionCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _cache;

    public CreateWorkItemDefinitionCommandHandler(IApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
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
        // Invalidate full cache and user-specific cache
        _cache.Remove("workitems_all");
        _cache.Remove($"workitems_{request.UserStoryId}");
        return entity.Id;
    }
}
