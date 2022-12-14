using System;
using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty()
                .WithMessage("{UserName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage($"UserName should not exceed 50 characters");

            RuleFor(e => e.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required");

            RuleFor(t => t.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice should not be empty")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greate than zero");
        }
    }
}

