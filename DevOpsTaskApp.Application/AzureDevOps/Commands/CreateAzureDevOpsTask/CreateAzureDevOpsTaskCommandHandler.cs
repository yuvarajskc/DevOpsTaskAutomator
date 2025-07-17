using DevOpsTaskApp.Application.Common.Interfaces;
using DevOpsTaskApp.Application.Common.Models;
using MediatR;

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
        var taskModel = new CreateAzureDevOpsTaskModel
        {
            Organization = request.Organization,
            Project = request.Project,
            Title = request.Title,
            AssignedTo = request.AssignedTo,
            IterationPath = request.IterationPath,
            UserStoryId = request.UserStoryId
        };

        var response = await _azureDevOpsService.CreateTaskAsync(request.PatToken, taskModel, cancellationToken);
        return response;

    }
}

