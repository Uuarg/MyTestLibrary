namespace MyTestLib.Models
{
    public class Author
    {
        public int Id { get; set; }           // Unique identifier for the author
        public string Name { get; set; } = string.Empty;      // Name of the author
        public string Nationality { get; set; } = string.Empty; // Nationality of the author
        public DateTime DateOfBirth { get; set; } // Date of birth of the author
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the author
    }
}