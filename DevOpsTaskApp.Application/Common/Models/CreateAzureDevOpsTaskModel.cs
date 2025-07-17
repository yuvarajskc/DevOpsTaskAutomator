using System;

namespace DevOpsTaskApp.Application.Common.Models;

public class CreateAzureDevOpsTaskModel
{
    public string Organization { get; set; } = default!;
    public string Project { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string AssignedTo { get; set; } = default!;
    public string IterationPath { get; set; } = default!;
    public string UserStoryId { get; set; } = default!;
}