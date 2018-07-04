using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProduct>
    {
        public CreateProductValidator()
        {
            this.RuleFor(p => p.Name)
                .NotNullOrWhiteSpace();

            this.RuleFor(p => p.Description)
                .NotNullOrWhiteSpace();

            this.RuleFor(p => p.Price)
                .GreaterThanOrEqualTo(0.00);

            this.RuleFor(p => p.Category)
                .NotNull()
                .SetValidator(new CategoryIdValidator());
        }
    }

    public static class NullOrWhiteSpaceValidator
    {
        public static IRuleBuilderOptions<T, string> NotNullOrWhiteSpace<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("{PropertyName} cannot be null, empty or whitespace.");
        }
    }
}
