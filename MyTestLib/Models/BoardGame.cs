namespace MyTestLib.Models
{
    public class BoardGame
    {
        public int Id { get; set; }           // Unique identifier for the board game
        public string Title { get; set; }     // Title of the board game
        public int DesignerID { get; set; } // ID of the designer of the board game
        public int PublishingYear { get; set; } // Year the board game was published
        public int PublisherID { get; set; }  // Foreign key to the Publisher entity
        public int GenreID { get; set; }      // Foreign key to the Genre entity
        public string Description { get; set; } = string.Empty;
        public string BGGUrl { get; set; } = string.Empty; // URL to the BoardGameGeek page for the board game
        public int MinimumPlayers { get; set; }
        public int MaximumPlayers { get; set; }
        public int PlayingTime { get; set; } // Playing time in minutes
        public string AgeRating { get; set; } = string.Empty; // Age rating for the board game
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the board game

    }
}