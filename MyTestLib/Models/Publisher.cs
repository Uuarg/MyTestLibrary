namespace MyTestLib.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; } = string.Empty; // URL to an image of the Publisher   
    }
}
