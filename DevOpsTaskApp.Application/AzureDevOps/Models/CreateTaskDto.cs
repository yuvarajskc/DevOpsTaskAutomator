using System;

namespace DevOpsTaskApp.Application.AzureDevOps.Models;

public class CreateTaskDto
{
    public string Organization { get; set; } = default!;
    public string Project { get; set; } = default!;
    public string IterationPath { get; set; } = default!;
    public string UserStoryId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string AssignedTo { get; set; } = default!;
}

