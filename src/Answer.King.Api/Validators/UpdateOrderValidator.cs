using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrder>
    {
        public UpdateOrderValidator()
        {
            this.RuleFor(o => o.Id)
                .NotEmpty();

            this.RuleFor(o => o.LineItems)
                .NotEmpty()
                .SetCollectionValidator(new LineItemValidator());
        }
    }
}