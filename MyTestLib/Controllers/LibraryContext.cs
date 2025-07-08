using Microsoft.EntityFrameworkCore;
using MyTestLib.Models; // Adjust namespace if needed

namespace MyTestLib.Controllers // Or use a separate namespace like MyTestLib.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Book { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Director> Director { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Designer> Designer { get; set; }
        public DbSet<Singer> Singer { get; set; }
        public DbSet<Record> Record { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Anime> Anime { get; set; }
        public DbSet<Shelf> Shelf { get; set; }
        public DbSet<BoardGame> BoardGame { get; set; }
    }
}