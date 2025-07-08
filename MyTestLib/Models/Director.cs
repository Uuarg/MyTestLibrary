namespace MyTestLib.Models
{
    public class Director
    {
        public int Id { get; set; }           // Unique identifier for the director
        public string Name { get; set; }      // Name of the director
        public string Nationality { get; set; } // Nationality of the director
        public DateTime DateOfBirth { get; set; } // Date of birth of the director
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the Director   
    }
}