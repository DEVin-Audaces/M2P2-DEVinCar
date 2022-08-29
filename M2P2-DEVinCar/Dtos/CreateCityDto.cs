using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Dtos
{
    public class CreateCityDto
    {
        [Required(ErrorMessage = "Campo Name de preenchimento obrigatório")]
        [StringLength(255)]
        public string Name { get; set; }
    }
}
