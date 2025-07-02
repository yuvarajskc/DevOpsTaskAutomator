using DevOpsTaskApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevOpsTaskApp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<WorkItemDefinition> WorkItemDefinitions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
