using authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace authentication.Context
{
    public class DatabaseContext :DbContext
    {
        protected readonly IConfiguration Configuration;

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        public DatabaseContext() : base(new DbContextOptionsBuilder<DatabaseContext>()
           .UseSqlServer(GetDefaultConnectionString()).Options)
        { }

       

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasKey(k => new { k.Id });
        }
        private static string GetDefaultConnectionString() =>
           new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build().GetConnectionString("DefaultConnection");

    }

}



