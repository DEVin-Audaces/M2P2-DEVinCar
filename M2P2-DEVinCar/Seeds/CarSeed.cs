using M2P2_DEVinCar.Models;

namespace M2P2_DEVinCar.Seeds
{
    public static class CarSeed
    {
        public static List<Car> CarSeeder { get; set; } = new List<Car>()
        {
            new Car
            {
                Id = 1,
                Name ="Punto",
                SuggestedPrice = 38000.00M
            },
            new Car
            {
                Id = 2,
                Name ="Prisma",
                SuggestedPrice = 42000.00M
            },
           new Car
            {
                Id = 3,
                Name ="Fusca",
                SuggestedPrice = 10000.00M
            },
             new Car
            {
                Id = 4,
                Name ="Kombi",
                SuggestedPrice = 8000.00M
            },
             new Car
            {
                Id = 5,
                Name ="TR-4",
                SuggestedPrice = 180000.00M
            },
             new Car
            {
                Id = 6,
                Name ="Camaro",
                SuggestedPrice = 308000.00M
            },
             new Car
            {
                Id = 7,
                Name ="Toro",
                SuggestedPrice = 138000.00M
            },
             new Car
            {
                Id = 8,
                Name ="Pulse",
                SuggestedPrice = 88000.00M
            },
             new Car
            {
                Id = 9,
                Name ="Nivus",
                SuggestedPrice = 78000.00M
            },
             new Car
            {
                Id = 10,
                Name ="Hilux",
                SuggestedPrice = 238000.00M
            }
                                                                        
        };
    }
}
