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
        if (_cache.TryGetValue(CacheKey, out List<WorkItemDefinitionDto> cachedResult))
        {
            Console.WriteLine("✅ Fetched from cache");
            return cachedResult;
        }

        var items = await _context.WorkItemDefinitions
            .Select(x => new WorkItemDefinitionDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                UserStoryId = x.UserStoryId,
                AssignedTo = x.AssignedTo
            })
            .ToListAsync(cancellationToken);

        _cache.Set(CacheKey, items, TimeSpan.FromMinutes(2)); // Optional expiry

        Console.WriteLine("📦 Fetched from DB and cached");

        return items;
    }
}
