using DevOpsTaskApp.Infrastructure.Persistence;
using DevOpsTaskApp.Application.WorkItemDefinitions.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DevOpsTaskApp.Application.Tests.WorkItemDefinitions.Queries;

[TestClass]
public class GetWorkItemDefinitionsQueryHandlerTest
{
    private ApplicationDbContext _context;

    [TestInitialize]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);


        _context.WorkItemDefinitions.AddRange(
            new Domain.Entities.WorkItemDefinition { Id = 1, Title = "Task 1", Description = "Desc", UserStoryId = "US123", AssignedTo = "yuvaraj" },
            new Domain.Entities.WorkItemDefinition { Id = 2, Title = "Task 2", Description = "Desc", UserStoryId = "US456", AssignedTo = "someone" }
        );

        await _context.SaveChangesAsync();
    }

    
    [TestMethod]
    public async Task Handle_FiltersByUserStoryId_WhenProvided()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var handler = new GetWorkItemDefinitionsQueryHandler(_context, memoryCache);
        var result = await handler.Handle(new GetWorkItemDefinitionsQuery { UserStoryId = "US123" }, CancellationToken.None);
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("US123", result.First().UserStoryId);
    }
    [TestMethod]
    public async Task Handle_ReturnsAll_WhenNoFilter()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var handler = new GetWorkItemDefinitionsQueryHandler(_context, memoryCache);
        var result = await handler.Handle(new GetWorkItemDefinitionsQuery(), CancellationToken.None);
        Assert.AreEqual(2, result.Count());
    }
}

