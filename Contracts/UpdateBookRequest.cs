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

    [StringLength(20)]
    public string ISBN { get; set; }

    [Required]
    public IFormFile? CoverImageUrl { get; set; }
}
}