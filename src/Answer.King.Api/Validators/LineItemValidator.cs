using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class LineItemValidator : AbstractValidator<LineItem>
    {
        public LineItemValidator()
        {
            this.RuleFor(c => c.Product)
                .NotNull()
                .SetValidator(new ProductIdValidator());

            this.RuleFor(c => c.Quantity)
                .GreaterThanOrEqualTo(0);
        }
    }

    public class ProductIdValidator : AbstractValidator<ProductId>
    {
        public ProductIdValidator()
        {
            this.RuleFor(c => c.Id)
                .NotEmpty();
        }
    }
}