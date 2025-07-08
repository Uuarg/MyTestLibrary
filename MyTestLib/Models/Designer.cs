namespace MyTestLib.Models
{
    public class Designer
    {
        public int Id { get; set; }           // Unique identifier for the designer
        public string Name { get; set; }      // Name of the designer
        public string Nationality { get; set; } // Nationality of the designer
        public DateTime DateOfBirth { get; set; } // Date of birth of the designer
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the Designer  
    }
}