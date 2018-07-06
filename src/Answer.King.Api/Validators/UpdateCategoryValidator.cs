using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategory>
    {
        public UpdateCategoryValidator()
        {
            this.RuleFor(c => c.Id)
                .NotEmpty();

            this.RuleFor(c => c.Name)
                .NotNullOrWhiteSpace();

            this.RuleFor(c => c.Description)
                .NotNullOrWhiteSpace();
        }
    }
}