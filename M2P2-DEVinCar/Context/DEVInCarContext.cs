using M2P2_DEVinCar.Models;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<SaleCar> SaleCars { get; set; }
    }
}
