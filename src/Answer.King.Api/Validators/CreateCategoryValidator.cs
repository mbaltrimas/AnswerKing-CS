using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategory>
    {
        public CreateCategoryValidator()
        {
            this.RuleFor(c => c.Name)
                .NotNullOrWhiteSpace();

            this.RuleFor(c => c.Description)
                .NotNullOrWhiteSpace();
        }
    }
}