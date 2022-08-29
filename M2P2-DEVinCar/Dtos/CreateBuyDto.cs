using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Dtos
{
    public class CreateBuyDto
    {
        [Required(ErrorMessage = "Campo SellerId de preenchimento obrigatório")]
        public int? SellerId { get; set; }

        public DateTime? SaleDate { get; set; }
    }
}
