namespace MyTestLib.Models
{
    public class Anime
    {
        public int Id { get; set; }           // Unique identifier for the anime
        public string Name { get; set; }     // Title of the anime
        public int DirectorID { get; set; }  // Author of the anime
        public int ReleaseYear { get; set; }   // Year the anime was released
        public int PublisherID { get; set; } // Foreign key to the Publisher entity
        public int GenreID { get; set; } // Foreign key to the Genre entity
        public string MyAnimeListUrl { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the anime
        public string Description { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int NumberOfEpisodes { get; set; } = 0;
    }
}