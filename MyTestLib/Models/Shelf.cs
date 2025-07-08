namespace MyTestLib.Models
{
    public class Shelf
    {
        public int Id { get; set; } // Unique identifier for the shelf
        public string Column { get; set; } = string.Empty; // Name of the shelf
        public string Row { get; set; } = string.Empty; // Description of the shelf
        public string Cell { get; set; } = string.Empty; // Description of the shelf
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the shelf
    }
}
