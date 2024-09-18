using System.ComponentModel.DataAnnotations.Schema;

public class ShoppingCartItem
{
    public Guid ShoppingCartItemId { get; set; }
    public Guid BookId { get; set; } // Foreign key
    public Book Book { get; set; }
    public int Quantity { get; set; }
    public string ShoppingCartId { get; set; }
    public Guid UserId { get; set; } 
}
