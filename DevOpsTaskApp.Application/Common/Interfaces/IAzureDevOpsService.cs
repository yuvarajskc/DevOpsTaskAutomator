using System;
using DevOpsTaskApp.Application.Common.Models;

namespace DevOpsTaskApp.Application.Common.Interfaces;

public interface IAzureDevOpsService
{
    Task<string> CreateTaskAsync(string patToken, CreateAzureDevOpsTaskModel model, CancellationToken cancellationToken);
}
