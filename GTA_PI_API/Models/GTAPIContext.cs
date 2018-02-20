using System.Data.Entity;

namespace GTA.PI.API.Models
{
    public class GTAPIContext : DbContext
    {
        public GTAPIContext() : base("name=GTAPIContext") { }
        public DbSet<User> Users { get; set; }
        public DbSet<ChooseSolution> ChooseSolutions { get; set; }

    }
}
