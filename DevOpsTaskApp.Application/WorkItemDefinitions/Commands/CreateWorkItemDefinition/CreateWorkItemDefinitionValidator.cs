using FluentValidation;

namespace DevOpsTaskApp.Application.WorkItemDefinitions.Commands;

public class CreateWorkItemDefinitionValidator : AbstractValidator<CreateWorkItemDefinitionCommand>
{
    public CreateWorkItemDefinitionValidator()
    {
RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.UserStoryId)
            .NotEmpty().WithMessage("UserStoryId is required");
        RuleFor(x => x.AssignedTo)
            .NotEmpty().WithMessage("AssignedTo is required");
    }
}
