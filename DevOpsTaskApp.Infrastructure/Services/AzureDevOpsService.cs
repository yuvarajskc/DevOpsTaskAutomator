using System;
using System.Text;
using System.Text.Json;
using DevOpsTaskApp.Application.AzureDevOps.Application.Common.Models;
using DevOpsTaskApp.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace DevOpsTaskApp.Infrastructure.Services;

public class AzureDevOpsService : IAzureDevOpsService
{
    private readonly HttpClient _httpClient;
    private readonly AzureDevOpsSettings _settings;

    public AzureDevOpsService(HttpClient httpClient, IOptions<AzureDevOpsSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task<string> CreateTaskAsync(string organization, string project, string title, string assignedTo, string iterationPath, string userStoryId, CancellationToken cancellationToken)
    {
        var patchDocument = new List<object>
        {
            new { op = "add", path = "/fields/System.Title", value = title },
            new { op = "add", path = "/fields/System.IterationPath", value = iterationPath },
            new { op = "add", path = "/fields/System.AssignedTo", value = assignedTo },
            new { op = "add", path = "/fields/System.Parent", value = userStoryId }
        };

        var url = $"{_settings.BaseUrl}{organization}/{project}/_apis/wit/workitems/$Task?api-version={_settings.ApiVersion}";
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(patchDocument), Encoding.UTF8, "application/json-patch+json")
        };

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
