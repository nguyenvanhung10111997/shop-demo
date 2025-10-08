using example.service.Features;
using FluentValidation;

namespace example.order.service.Features.Orders.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderDetails).NotNull().NotEmpty();

        When(x => x.OrderDetails is { Count: > 0 }, () =>
        {
            RuleForEach(x => x.OrderDetails).ChildRules(details =>
            {
                details.RuleFor(d => d.ProductId).NotEqual(Guid.Empty);
                details.RuleFor(d => d.ProductCode).NotEmpty().MaximumLength(50);
                details.RuleFor(d => d.ProductName).NotEmpty().MaximumLength(200);
                details.RuleFor(d => d.Quantity).GreaterThan(0);
                details.RuleFor(d => d.Amount).GreaterThan(0);
            });
        });
    }
}
