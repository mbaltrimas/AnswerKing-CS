using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            this.RuleFor(o => o.LineItems)
                .NotEmpty()
                .SetCollectionValidator(new LineItemValidator());
        }
    }
}