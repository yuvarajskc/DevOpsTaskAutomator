using System;
using System.Text;
using System.Text.Json;
using DevOpsTaskApp.Application.AzureDevOps.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Options;

namespace DevOpsTaskApp.Application.AzureDevOps.Commands.CreateAzureDevOpsTask;

public class CreateAzureDevOpsTaskCommandHandler : IRequestHandler<CreateAzureDevOpsTaskCommand, string>
{
    private readonly HttpClient _httpClient;
    private readonly AzureDevOpsSettings _settings;

    public CreateAzureDevOpsTaskCommandHandler(HttpClient httpClient, IOptions<AzureDevOpsSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }


    public async Task<string> Handle(CreateAzureDevOpsTaskCommand request, CancellationToken cancellationToken)
    {
        var jsonContent = new
        {
            op = "add",
            path = "/fields/System.Title",
            value = request.Title
        };

        var patchDocument = new List<object>
        {
            new { op = "add", path = "/fields/System.Title", value = request.Title },
            new { op = "add", path = "/fields/System.IterationPath", value = request.IterationPath },
            new { op = "add", path = "/fields/System.AssignedTo", value = request.AssignedTo },
            new { op = "add", path = "/fields/System.Parent", value = request.UserStoryId }
        };

        var url = $"{_settings.BaseUrl}{request.Organization}/{request.Project}/_apis/wit/workitems/$Task?api-version={_settings.ApiVersion}";

        var message = new HttpRequestMessage(HttpMethod.Post, url);
        message.Content = new StringContent(JsonSerializer.Serialize(patchDocument), Encoding.UTF8, "application/json-patch+json");

        var response = await _httpClient.SendAsync(message, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }
}

