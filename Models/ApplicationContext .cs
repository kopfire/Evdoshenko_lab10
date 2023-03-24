using Evdoshenko_lab10.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Evdoshenko_lab10.Models
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Teacher> Teacher { get; set; }

        public DbSet<Progress> Progress { get; set; }

        public DbSet<Achievement> Achievement { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            /*Database.EnsureDeleted();
            Database.EnsureCreated();*/
        }
    }
}
