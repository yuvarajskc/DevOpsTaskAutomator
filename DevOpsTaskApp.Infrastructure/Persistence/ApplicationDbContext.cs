using DevOpsTaskApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DevOpsTaskApp.Application.Common.Interfaces; // Add this line if the interface exists in this namespace

namespace DevOpsTaskApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<WorkItemDefinition> WorkItemDefinitions => Set<WorkItemDefinition>();
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);
}
