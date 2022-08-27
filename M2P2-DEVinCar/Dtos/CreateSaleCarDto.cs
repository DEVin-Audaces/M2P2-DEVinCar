using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Dtos
{
    public class CreateSaleCarDto
    {
        [Required(ErrorMessage = "Campo CarId de preenchimento obrigatório")]
        public int? CarId { get; set; }

        public decimal? UnitPrice { get; set; }

        public int? Amount { get; set; }
    }
}
