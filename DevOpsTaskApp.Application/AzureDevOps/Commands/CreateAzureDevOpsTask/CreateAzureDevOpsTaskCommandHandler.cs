using System;
using System.Text;
using System.Text.Json;
using DevOpsTaskApp.Application.AzureDevOps.Application.Common.Models;
using DevOpsTaskApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevOpsTaskApp.Application.AzureDevOps.Commands.CreateAzureDevOpsTask;

public class CreateAzureDevOpsTaskCommandHandler : IRequestHandler<CreateAzureDevOpsTaskCommand, string>
{
    private readonly IAzureDevOpsService _azureDevOpsService;

    public CreateAzureDevOpsTaskCommandHandler(IAzureDevOpsService azureDevOpsService)
    {
        _azureDevOpsService = azureDevOpsService;
    }

    public async Task<string> Handle(CreateAzureDevOpsTaskCommand request, CancellationToken cancellationToken)
    {
        return await _azureDevOpsService.CreateTaskAsync(
            request.Organization,
            request.Project,
            request.Title,
            request.AssignedTo,
            request.IterationPath,
            request.UserStoryId,
            cancellationToken);
    }
}

