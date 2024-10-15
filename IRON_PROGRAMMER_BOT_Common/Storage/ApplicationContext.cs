using IRON_PROGRAMMER_BOT_Common.Models;
using Microsoft.EntityFrameworkCore;

namespace IRON_PROGRAMMER_BOT_Common.Storage
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Appoint> Appoints { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) => Database.Migrate();

    }
}
