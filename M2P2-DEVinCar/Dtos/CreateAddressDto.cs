using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Dtos
{
    public class CreateAddressDto
    {
        [Required(ErrorMessage = "Campo Street de preenchimento obrigatório")]
        [StringLength(150)]
        public string Street { get; set; }

        [Required(ErrorMessage = "Campo Number de preenchimento obrigatório")]
        public int Number { get; set; }

        [StringLength(255)]
        public string? Complement { get; set; }

        [Required(ErrorMessage = "Campo Cep de preenchimento obrigatório")]
        [StringLength(8)]
        public string Cep { get; set; }
    }
}
