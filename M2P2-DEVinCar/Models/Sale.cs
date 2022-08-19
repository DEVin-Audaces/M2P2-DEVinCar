using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo BuyerId de preenchimento obrigatório")]
        public int? BuyerId { get; set; }

        [Required(ErrorMessage = "Campo SellerId de preenchimento obrigatório")]
        public int? SellerId { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Formato de data inválido")]
        public DateTime? SaleDate { get; set; }

        public virtual User? Buyer { get; set; }

        public virtual User? Seller { get; set; }
    }
}