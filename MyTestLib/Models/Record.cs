namespace MyTestLib.Models
{
    public class Record
    {
        public int Id { get; set; }           // Unique identifier for the record
        public string Name { get; set; }     // Title of the record
        public int singerID { get; set; }    // Author of the record
        public int PublishingYear { get; set; }         // Year the record was published
        public int PublisherID { get; set; } // Foreign key to the Publisher entity
        public int GenreID { get; set; } // Foreign key to the Genre entity
        public string SpotifyURL { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the record
    }
}