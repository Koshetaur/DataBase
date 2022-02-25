using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibBase
{
    public class ApplicationDbContextOptions : DbContextOptions<ApplicationContext>
    {
        public string ConnectionString { get; set; }
    }

    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var opt = new ApplicationDbContextOptions
            {
                ConnectionString = Helper.ConnectionString
            };

            //var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>(opt);
            return new ApplicationContext(opt);
        }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reserve> Reservs { get; set; }
        public string DbPath { get; }
        public ApplicationContext(ApplicationDbContextOptions opt) : base(opt)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var opt = (ApplicationDbContextOptions)optionsBuilder.Options;
            optionsBuilder.UseSqlite(opt.ConnectionString);
        }
    }
}
