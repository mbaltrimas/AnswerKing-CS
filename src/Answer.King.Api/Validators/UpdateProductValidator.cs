﻿using Answer.King.Api.ViewModels;
using FluentValidation;

namespace Answer.King.Api.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProduct>
    {
        public UpdateProductValidator()
        {
            this.RuleFor(p => p.Id)
                .NotEmpty();

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
}