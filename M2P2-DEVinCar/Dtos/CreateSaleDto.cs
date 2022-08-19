using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Dtos
{
    public class CreateSaleDto
    {
        [Required(ErrorMessage="Campo BuyerId de preenchimento obrigatório")]
        public int? BuyerId { get; set;}
    
        public DateTime? SaleDate { get; set;}

    }
}