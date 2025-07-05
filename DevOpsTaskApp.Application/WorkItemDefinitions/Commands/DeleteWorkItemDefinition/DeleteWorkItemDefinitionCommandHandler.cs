using System;
using DevOpsTaskApp.Application.Common.Exceptions;
using DevOpsTaskApp.Application.Common.Interfaces;
using DevOpsTaskApp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands.DeleteWorkItemDefinition;

public class DeleteWorkItemDefinitionCommandHandler : IRequestHandler<DeleteWorkItemDefinitionCommand,Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _cache;

    public DeleteWorkItemDefinitionCommandHandler(IApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Unit> Handle(DeleteWorkItemDefinitionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.WorkItemDefinitions.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(WorkItemDefinition), request.Id);

        _context.WorkItemDefinitions.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        // Invalidate cache
        _cache.Remove("workitems_all");
        _cache.Remove($"workitems_{entity.UserStoryId}");

        return Unit.Value;
    }
}

