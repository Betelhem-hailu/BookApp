using System.ComponentModel.DataAnnotations;

namespace BookStore.Contracts{
public class CreateBookRequest
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
     // This property will hold the file data if the image is being uploaded
    public IFormFile? ImageFile { get; set; }

    public List<string> Categories { get; set; }
}
}