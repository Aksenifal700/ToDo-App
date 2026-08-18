using FluentValidation;
using TodoApp.Interfaces.DTOs.Category;

namespace TodoApp.BusinessLogic.Validators;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);
    }
}