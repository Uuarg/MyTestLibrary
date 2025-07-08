namespace MyTestLib.Models
{
    public class Movie
    {
        public int Id { get; set; }           // Unique identifier for the movie
        public string Name { get; set; }     // Title of the movie
        public int directorID { get; set; }    // Foreign key to the Director entity
        public int PublishingYear { get; set; }         // Year the movie was published
        public int PublisherID { get; set; } // Foreign key to the Publisher entity
        public int GenreID { get; set; } // Foreign key to the Genre entity
        public string IMDB { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the Movie   
        public string Description { get; set; } = string.Empty;
    }
}