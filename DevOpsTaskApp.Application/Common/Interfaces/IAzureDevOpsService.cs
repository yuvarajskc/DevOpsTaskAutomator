using System;

namespace DevOpsTaskApp.Application.Common.Interfaces;

public interface IAzureDevOpsService
{
    Task<string> CreateTaskAsync(string organization, string project, string title, string assignedTo, string iterationPath, string userStoryId, CancellationToken cancellationToken);
}
