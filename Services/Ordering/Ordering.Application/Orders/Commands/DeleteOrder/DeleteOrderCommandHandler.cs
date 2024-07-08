namespace Ordering.Application.Orders.Commands.DeleteOrder;

// The DeleteOrderHandler handles the DeleteOrderCommand
public class DeleteOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        // Extract the Order ID from the command
        var orderId = OrderId.Of(command.OrderId);

        // Retrieve the existing order from the database (Orders Table) using the Order ID
        var order = await dbContext.Orders
            .FindAsync([orderId], cancellationToken: cancellationToken);

        // If the order is not found, throw an OrderNotFoundException
        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }

        // Remove the order from the database context
        dbContext.Orders.Remove(order);

        // Save changes to the database
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return the result indicating the deletion was successful
        return new DeleteOrderResult(true);
    }
}
