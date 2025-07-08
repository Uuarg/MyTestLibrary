namespace MyTestLib.Models
{
    public class Singer
    {
        public int Id { get; set; }           // Unique identifier for the singer
        public string Name { get; set; }      // Name of the singer
        public string Nationality { get; set; } // Nationality of the singer
        public DateTime DateOfBirth { get; set; } // Date of birth of the singer
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the singer
    }
}