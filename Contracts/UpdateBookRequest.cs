using System.ComponentModel.DataAnnotations;

namespace BookStore.Contracts{
public class UpdateBookRequest
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(100)]
    public string Author { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    [StringLength(20)]
    public string ISBN { get; set; }

    public string Language {get; set;}

    [Required]
    public DateTime PublishDate {get; set;}

    // [Required]
    public IFormFile? ImageFile { get; set; }  // For the new image file (nullable)

    public string? ImageUrl { get; set; }  // For keeping the existing URL if no new image is provided

    public List<string> Categories { get; set; }
}
}