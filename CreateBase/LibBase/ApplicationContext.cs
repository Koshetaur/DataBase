using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibBase
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseSqlite(Helper.ConnectionString);
            return new ApplicationContext(builder.Options);
        }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reserve> Reservs { get; set; }
        public string DbPath { get; }
        public ApplicationContext(DbContextOptions<ApplicationContext> opt) : base(opt)
        {
        }
    }
}
