﻿namespace Basket.API.Basket.StoreBasket;

// Define a command and result records for storing a basket, using a primary constructor
public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

// Define a validator for the StoreBasketCommand using FluentValidation.
public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    // Constructor where validation rules are defined.
    public StoreBasketCommandValidator()
    {
        // Ensure Cart is not null, with a custom error message.
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");

        // Ensure Cart's UserName is not empty, with a custom error message.
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

// Define a handler for the StoreBasketCommand, using a primary constructor to inject the repository dependency.
public class StoreBasketCommandHandler(IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    // Handle method to process the StoreBasketCommand.
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        // Apply discounts to the items in the cart before storing it.
        await DeductDiscount(command.Cart, cancellationToken);

        // Store the basket asynchronously in the repository.
        await repository.StoreBasketAsync(command.Cart, cancellationToken);

        // Return the result containing the UserName from the Cart.
        return new StoreBasketResult(command.Cart.UserName);
    }

    
    // Private method to deduct discount, demonstrating an optional step.
    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        // Communicate with Discount.Grpc to calculate the latest prices of products in the shopping cart.
        foreach (var item in cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
    
}
