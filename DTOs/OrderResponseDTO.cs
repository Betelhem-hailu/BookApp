using BookStore.DTOs;

namespace BookStore.DTOs {
    public class OrderResponseDTO
    {
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
}

public class OrderItemDTO
{
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } // Assuming you want to display the book title
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string CoverImageUrl { get; set; }
}

}