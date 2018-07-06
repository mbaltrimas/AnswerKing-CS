using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrder>
    {
        public UpdateOrderValidator()
        {
            this.RuleFor(c => c.Id)
                .NotEmpty();

            this.RuleFor(c => c.LineItems)
                .NotEmpty()
                .SetCollectionValidator(new LineItemValidator());
        }
    }
}