using System.Data.Entity;
using System.Diagnostics;

namespace BookService.Models
{
    public class BookServiceContext : DbContext
    {
        public BookServiceContext()
            : base("name=BookServiceContext")
        {
            this.Database.Log = s => Debug.WriteLine(s);
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
