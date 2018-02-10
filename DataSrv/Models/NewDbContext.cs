using System.Data.Entity;

namespace DataSrv.Models
{
    public class NewDbContext : DbContext
    {
        public NewDbContext() : base("name=NewDbContext") { }
        public DbSet<Movie> Movie { get; set; }

    }
}
