using System;

namespace DevOpsTaskApp.Application.AzureDevOps.Application.Common.Models;

public class AzureDevOpsSettings
{
    public string BaseUrl { get; set; } = default!;
    public string ApiVersion { get; set; } = "7.0"; 
    public string PersonalAccessToken { get; set; } = default!; 
}

