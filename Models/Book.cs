public class Book
{
    public Guid BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description {get; set;}
    public decimal Price { get; set; }
    public string ISBN { get; set; }
    public string Language {get; set;}
    public DateTime PublishDate {get; set;}
    public string CoverImageUrl { get; set; } 
    public ICollection<OrderItem> OrderItems { get; set; }
    public List<Category> Categories { get; set; } = new();
}
