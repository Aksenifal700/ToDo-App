using FluentValidation;
using TodoApp.Interfaces.DTOs.TaskItem;
using TodoApp.Interfaces.Entities;

namespace TodoApp.BusinessLogic.Validators;

public class CreateTaskItemValidator : AbstractValidator<CreateTaskItemDto>
{
    public CreateTaskItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .When(x => x.Description is not null);

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.DueDate.HasValue)
            .WithMessage("Due date cannot be in the past");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .When(x => x.CategoryId.HasValue)
            .WithMessage("CategoryId cannot be an empty guid");
    }
}