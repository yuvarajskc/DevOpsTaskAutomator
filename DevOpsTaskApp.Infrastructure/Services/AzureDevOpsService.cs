using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DevOpsTaskApp.Application.AzureDevOps.Application.Common.Models;
using DevOpsTaskApp.Application.Common.Interfaces;
using DevOpsTaskApp.Application.Common.Models;
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

    public async Task<string> CreateTaskAsync(string patToken, CreateAzureDevOpsTaskModel model, CancellationToken cancellationToken)
    {
        var patchDocument = new List<object>
        {
            new { op = "add", path = "/fields/System.Title", value = model.Title },
            new { op = "add", path = "/fields/System.IterationPath", value = model.IterationPath },
            new { op = "add", path = "/fields/System.AssignedTo", value = model.AssignedTo },
            new { op = "add", path = "/fields/System.Parent", value = model.UserStoryId }
        };

        var url = $"{_settings.BaseUrl}{model.Organization}/{model.Project}/_apis/wit/workitems/$Task?api-version={_settings.ApiVersion}";

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(patchDocument), Encoding.UTF8, "application/json-patch+json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($":{patToken}")));

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
