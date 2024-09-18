using System.ComponentModel.DataAnnotations.Schema;

public class OrderItem
{
    public Guid OrderItemId { get; set; }
    
    public Guid OrderId { get; set; }
    
    [ForeignKey("OrderId")]
    public Order Order { get; set; }  // Navigation property to Order

    public Guid BookId { get; set; }
    
    [ForeignKey("BookId")]
    public Book Book { get; set; }  // Navigation property to Book

    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
}
