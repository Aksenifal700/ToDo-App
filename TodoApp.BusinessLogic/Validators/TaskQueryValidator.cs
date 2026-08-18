using FluentValidation;
using TodoApp.Interfaces.DTOs.Common;

namespace TodoApp.BusinessLogic.Validators;

public class TaskQueryValidator : AbstractValidator<TaskQueryDto>
{
    public TaskQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .When(x => x.SearchTerm is not null);

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .When(x => x.CategoryId.HasValue)
            .WithMessage("CategoryId cannot be an empty guid");
    }
}