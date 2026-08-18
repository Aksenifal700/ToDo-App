using FluentValidation;
using TodoApp.Interfaces.DTOs.Category;

namespace TodoApp.BusinessLogic.Validators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}