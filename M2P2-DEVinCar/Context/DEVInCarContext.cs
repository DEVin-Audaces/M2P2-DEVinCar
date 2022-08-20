using M2P2_DEVinCar.Models;
using Microsoft.EntityFrameworkCore;
using M2P2_DEVinCar.Seeds;

namespace M2P2_DEVinCar.Context
{
    public class DEVInCarContext : DbContext
    {
        public DEVInCarContext()
        {
        }

        public DEVInCarContext(DbContextOptions<DEVInCarContext> options)
            :base(options)
        {
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set;}
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleCar> SaleCars { get; set; }
        public DbSet<State> States { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                        .HasOne(r => r.Buyer)
                        .WithMany()
                        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Sale>()
                       .HasOne(r => r.Seller)
                       .WithMany()
                       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<State>().HasData(StateSeed.StateSeeder);
        }
    }
}
