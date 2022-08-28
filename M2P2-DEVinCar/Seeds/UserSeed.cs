using M2P2_DEVinCar.Models;

namespace M2P2_DEVinCar.Seeds
{
    public static class UserSeed
    {

        public static List<User> UserSeeder { get; set; } = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Matheus Gevartosky",
                Email = "matheus.gevartosky@senai.com",
                BirthDate = DateTime.Parse("01/01/1991"),
                Password = "123456789"
            },
            new User
            {
                Id = 2,
                Name = "Rodrigo Raiche",
                Email = "rodrigo.raiche@senai.com",
                BirthDate = DateTime.Parse("01/01/1990"),
                Password = "123456789"
            },
            new User
            {
                Id = 3,
                Name = "Lucas Reibnitz",
                Email = "lucas.reibnitz@senai.com",
                BirthDate = DateTime.Parse("01/01/1993"),
                Password = "123456789"
            },
            new User
            {
                Id = 4,
                Name = "Alessandra Soares",
                Email = "alessandra.soares@senai.com",
                BirthDate = DateTime.Parse("01/01/1992"),
                Password = "123456789"
            }
        };
    }
}
