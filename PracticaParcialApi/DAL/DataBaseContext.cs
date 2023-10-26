using Microsoft.EntityFrameworkCore;
using PracticaParcialApi.DAL.Entities;
using System.Diagnostics.Metrics;
using System.Drawing;

namespace PracticaParcialApi.DAL
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Owner>().HasIndex(o => o.Telefono).IsUnique();
            modelBuilder.Entity<Animal>().HasIndex("Name", "OwnerId").IsUnique(); 
        }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Animal> Animals { get; set; }

    }
}
