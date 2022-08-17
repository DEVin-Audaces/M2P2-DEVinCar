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
        
        public DbSet<CarModel> CarModel { get; set; }
    }
}
