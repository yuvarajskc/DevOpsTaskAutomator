// File: CreateWorkItemDefinitionCommandHandlerTests.cs
using DevOpsTaskApp.Application.WorkItemDefinitions.Commands;
using DevOpsTaskApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevOpsTaskApp.Application.Tests.WorkItemDefinitions.Commands
{
    [TestClass]
    public class CreateWorkItemDefinitionCommandHandlerTests
    {
        private ApplicationDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestCreateDb")
                .Options;

            _context = new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task Handle_ShouldCreateWorkItemDefinition()
        {
            var memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
            var handler = new CreateWorkItemDefinitionCommandHandler(_context, memoryCache);

            var command = new CreateWorkItemDefinitionCommand
            {
                Title = "Test Task",
                Description = "Some description",
                UserStoryId = "US123",
                AssignedTo = "yuvaraj"
            };

            var resultId = await handler.Handle(command, CancellationToken.None);

            var createdItem = await _context.WorkItemDefinitions.FindAsync(resultId);

            Assert.IsNotNull(createdItem);
            Assert.AreEqual("Test Task", createdItem.Title);
        }
    }
}
