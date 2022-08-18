using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M2P2_DEVinCar.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("City")]
        public int CityId { get; set; }

        [Required]
        [StringLength(150)]
        public string Street { get; set; }

        [Required]
        [StringLength(8)]
        public string Cep { get; set; }

        [Required]
        public int Number { get; set; }

        [StringLength(255)]
        public string? Complement { get; set; }

        public City? City { get; set; }
    }
}