namespace Discount.Grpc.Models;
public class Coupon
{
    // Unique identifier for the coupon
    public int Id { get; set; }

    // Name of the product that the coupon applies to
    public string ProductName { get; set; } = default!;

    // Description of the coupon
    public string Description { get; set; } = default!;

    // Discount amount provided by the coupon
    public int Amount { get; set; }
}
