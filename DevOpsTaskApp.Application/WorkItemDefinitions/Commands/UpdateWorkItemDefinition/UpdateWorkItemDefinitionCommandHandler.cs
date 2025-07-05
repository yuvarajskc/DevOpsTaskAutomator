using System;
using DevOpsTaskApp.Application.Common.Interfaces;
using DevOpsTaskApp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands.UpdateWorkItemDefinition;

public class UpdateWorkItemDefinitionCommandHandler : IRequestHandler<UpdateWorkItemDefinitionCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _cache;

    public UpdateWorkItemDefinitionCommandHandler(IApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Unit> Handle(UpdateWorkItemDefinitionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.WorkItemDefinitions.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null)
            throw new KeyNotFoundException($"WorkItemDefinition with ID {request.Id} not found.");

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.UserStoryId = request.UserStoryId;
        entity.AssignedTo = request.AssignedTo;

        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate related cache
        _cache.Remove("workitems_all");
        _cache.Remove($"workitems_{request.UserStoryId}");

        return Unit.Value;
    }
}

