using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrder>
    {
        public CreateOrderValidator()
        {
            this.RuleFor(c => c.LineItems)
                .NotEmpty()
                .SetCollectionValidator(new LineItemValidator());
        }
    }
}