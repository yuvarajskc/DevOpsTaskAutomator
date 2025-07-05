using System;
using FluentValidation;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands.UpdateWorkItemDefinition;

public class UpdateWorkItemDefinitionValidator : AbstractValidator<UpdateWorkItemDefinitionCommand>
{
    public UpdateWorkItemDefinitionValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.UserStoryId).NotEmpty();
        RuleFor(x => x.AssignedTo).NotEmpty();
    }
}

