using DevOpsTaskApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Queries;

public class GetWorkItemDefinitionsQueryHandler : IRequestHandler<GetWorkItemDefinitionsQuery, List<WorkItemDefinitionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    private const string CacheKey = "workitemdefinitions_cache";

    public GetWorkItemDefinitionsQueryHandler(IApplicationDbContext context, IMemoryCache cache, IMapper mapper)
    {
        _context = context;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<List<WorkItemDefinitionDto>> Handle(GetWorkItemDefinitionsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"workitems_{request.UserStoryId ?? "all"}";

        if (_cache.TryGetValue(cacheKey, out List<Domain.Entities.WorkItemDefinition>? cachedResult))
        {
            Console.WriteLine("✅ Fetched from cache");
            return _mapper.Map<List<WorkItemDefinitionDto>>(cachedResult);
        }

        var query = _context.WorkItemDefinitions
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.UserStoryId))
        {
            query = query.Where(w => w.UserStoryId == request.UserStoryId);
        }

        var result = await query.Skip((request.Page - 1) * request.PageSize)
.Take(request.PageSize).ToListAsync(cancellationToken);

        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(2));
        Console.WriteLine("📦 Cached filtered result");
        return _mapper.Map<List<WorkItemDefinitionDto>>(result);
    }
}
