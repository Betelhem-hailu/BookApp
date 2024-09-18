using BookStore.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    
    public List<OrderItem> OrderItems { get; set; }
}
