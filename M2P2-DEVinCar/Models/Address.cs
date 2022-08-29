using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M2P2_DEVinCar.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo CityId de preenchimento obrigatório")]
        [ForeignKey("City")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Campo Street de preenchimento obrigatório")]
        [StringLength(150)]
        public string Street { get; set; }

        [Required(ErrorMessage = "Campo Cep de preenchimento obrigatório")]
        [StringLength(8)]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Campo Number de preenchimento obrigatório")]
        public int Number { get; set; }

        [StringLength(255)]
        public string? Complement { get; set; }

        public City? City { get; set; }
    }
}