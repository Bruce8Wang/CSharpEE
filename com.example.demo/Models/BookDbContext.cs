using System.Data.Entity;
using System.Diagnostics;

namespace com.example.demo.Models
{
    public class BookDbContext : DbContext
    {
        public BookDbContext() : base("name=BookDbContext")
        {
            Database.Log = s => Debug.WriteLine(s);
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
