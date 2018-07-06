using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            this.RuleFor(c => c.Amount)
                .GreaterThanOrEqualTo(0.00);

            this.RuleFor(c => c.OrderId)
                .NotEmpty();
        }
    }
}