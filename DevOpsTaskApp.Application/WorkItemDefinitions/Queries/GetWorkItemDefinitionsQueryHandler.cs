using DevOpsTaskApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Queries;

public class GetWorkItemDefinitionsQueryHandler : IRequestHandler<GetWorkItemDefinitionsQuery, List<WorkItemDefinitionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "workitemdefinitions_cache";

    public GetWorkItemDefinitionsQueryHandler(IApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<WorkItemDefinitionDto>> Handle(GetWorkItemDefinitionsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"workitems_{request.UserStoryId ?? "all"}";

        if (_cache.TryGetValue(cacheKey, out List<WorkItemDefinitionDto> cachedResult))
        {
            Console.WriteLine("✅ Fetched from cache");
            return cachedResult;
        }

        var query = _context.WorkItemDefinitions
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.UserStoryId))
        {
            query = query.Where(w => w.UserStoryId == request.UserStoryId);
        }

        var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new WorkItemDefinitionDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    UserStoryId = x.UserStoryId,
                    AssignedTo = x.AssignedTo
                })
                .ToListAsync(cancellationToken);

        _cache.Set(cacheKey, items, TimeSpan.FromMinutes(2));
        Console.WriteLine("📦 Cached filtered result");

        return items;
    }
}
