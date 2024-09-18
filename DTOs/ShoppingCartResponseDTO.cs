using BookStore.DTOs;

namespace BookStore.DTOs {
    public class ShoppingCartDTO
{
    public List<ShoppingCartItemDTO> Items { get; set; }
    public decimal Total { get; set; }
}

public class ShoppingCartItemDTO
{
    public Guid ShoppingCartItemId { get; set; }
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } // Add other necessary properties
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string CoverImageUrl { get; set; }
}
}