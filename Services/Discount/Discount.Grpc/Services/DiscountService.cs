namespace Discount.Grpc.Services;

// This class provides gRPC service implementations for managing discount coupons.
// It includes methods for retrieving, creating, updating, and deleting discounts from the database.
public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{

    // Override method to retrieve a discount based on the product name
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("Starting GetDiscount for ProductName: {ProductName}", request.ProductName);

        // Retrieve the coupon from the database based on the product name
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        // If no coupon is found, create a default "No Discount" coupon
        if (coupon is null)
        {
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            logger.LogWarning("No discount found for ProductName: {ProductName}. Returning default coupon.", request.ProductName);
        }

        // Map the coupon to the CouponModel and return it
        var couponModel = coupon.Adapt<CouponModel>();

        logger.LogInformation("Completed GetDiscount for ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
        return couponModel;
    }

    // Override method to create a new discount
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("Starting CreateDiscount for ProductName: {ProductName}", request.Coupon.ProductName);

        // Map the request coupon model to the Coupon entity
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        // Add the new coupon to the database
        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount successfully created for ProductName: {ProductName}", coupon.ProductName);

        // Map the coupon to the CouponModel and return it
        var couponModel = coupon.Adapt<CouponModel>();

        logger.LogInformation("Completed CreateDiscount for ProductName: {ProductName}", coupon.ProductName);
        return couponModel;
    }

    // Override method to update an existing discount
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("Starting UpdateDiscount for ProductName: {ProductName}", request.Coupon.ProductName);

        // Map the request coupon model to the Coupon entity
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        // Update the existing coupon in the database
        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount successfully updated for ProductName: {ProductName}", coupon.ProductName);

        // Map the coupon to the CouponModel and return it
        var couponModel = coupon.Adapt<CouponModel>();

        logger.LogInformation("Completed UpdateDiscount for ProductName: {ProductName}", coupon.ProductName);
        return couponModel;
    }

    // Override method to delete a discount based on the product name
    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("Starting DeleteDiscount for ProductName: {ProductName}", request.ProductName);

        // Retrieve the coupon from the database based on the product name
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        // If no coupon is found, throw an exception
        if (coupon is null)
        {
            logger.LogWarning("Discount not found for ProductName: {ProductName}", request.ProductName);
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
        }

        // Remove the coupon from the database
        dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount successfully deleted for ProductName: {ProductName}", request.ProductName);

        // Return a response indicating successful deletion
        logger.LogInformation("Completed DeleteDiscount for ProductName: {ProductName}", request.ProductName);
        return new DeleteDiscountResponse { Success = true };
    }
}
