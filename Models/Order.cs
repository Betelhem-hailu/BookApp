using BookStore.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
