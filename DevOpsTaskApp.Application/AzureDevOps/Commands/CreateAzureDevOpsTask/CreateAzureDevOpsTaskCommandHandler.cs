using System;
using System.Text;
using System.Text.Json;
using DevOpsTaskApp.Application.AzureDevOps.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevOpsTaskApp.Application.AzureDevOps.Commands.CreateAzureDevOpsTask;

public class CreateAzureDevOpsTaskCommandHandler : IRequestHandler<CreateAzureDevOpsTaskCommand, string>
{
    private readonly HttpClient _httpClient;
    private readonly AzureDevOpsSettings _settings;
    private readonly ILogger<CreateAzureDevOpsTaskCommandHandler> _logger;

    public CreateAzureDevOpsTaskCommandHandler(
        HttpClient httpClient,
        IOptions<AzureDevOpsSettings> options,
        ILogger<CreateAzureDevOpsTaskCommandHandler> logger)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<string> Handle(CreateAzureDevOpsTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {


           var pat = _settings.PersonalAccessToken;
           var base64Pat = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{pat}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Pat);
            // Uncomment the line below if you want to use a custom header instead of Basic Auth
            

            var patchDocument = new List<object>
        {
            new { op = "add", path = "/fields/System.Title", value = request.Title },
            new { op = "add", path = "/fields/System.IterationPath", value = request.IterationPath },
            new { op = "add", path = "/fields/System.AssignedTo", value = request.AssignedTo },
            new { op = "add", path = "/fields/System.Parent", value = request.UserStoryId }
        };

            var url = $"{_settings.BaseUrl}{request.Organization}/{request.Project}/_apis/wit/workitems/$Task?api-version={_settings.ApiVersion}";

            var message = new HttpRequestMessage(HttpMethod.Post,url)
            {
                Content = new StringContent(JsonSerializer.Serialize(patchDocument), Encoding.UTF8, "application/json-patch+json")
            };

            var response = await _httpClient.SendAsync(message, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var status = (int)response.StatusCode;
                // 🔴 You can inject ILogger<CreateAzureDevOpsTaskCommandHandler> to log this
                throw new ApplicationException($"Azure DevOps API returned error {status}: {error}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    catch (Exception ex)
    {
        // Consider injecting ILogger<T> and logging like:
        _logger.LogError(ex, "Azure DevOps Task creation failed for Title: {Title}", request.Title);
        throw; // Rethrow the exception to ensure all code paths return a value
    }
}

}

