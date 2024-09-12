using BookStore.DTOs;

namespace BookStore.DTOs {
    public class BookResponseDTO
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
        public List<string> Categories { get; set; }
}
}