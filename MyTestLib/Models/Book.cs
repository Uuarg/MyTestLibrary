namespace MyTestLib.Models
{
    public class Book
    {
        public int Id { get; set; }           // Unique identifier for the book
        public string Name { get; set; }     // Title of the book
        public int AuthorId { get; set; }    // Author of the book
        public int PublishingYear { get; set; }         // Year the book was published
        public int PublisherID { get; set; } // Foreign key to the Publisher entity
        public int GenreID { get; set; } // Foreign key to the Genre entity
        public string ISBN { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the book   

    }
}