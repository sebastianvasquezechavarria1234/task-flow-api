using FluentValidation;
using MiAp.Models;

namespace MiAp.Validators;

public class TaskItemValidator : AbstractValidator<TaskItem>
{
    public TaskItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(status => new[] { "todo", "in-progress", "done" }.Contains(status.ToLower()))
            .WithMessage("Status must be either 'todo', 'in-progress', or 'done'.");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority is required.")
            .Must(priority => new[] { "low", "medium", "high" }.Contains(priority.ToLower()))
            .WithMessage("Priority must be either 'low', 'medium', or 'high'.");
    }
}
