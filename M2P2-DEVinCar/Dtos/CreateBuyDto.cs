using System.ComponentModel.DataAnnotations;

namespace University.Dtos
{
    public class CreateBuyDto
    {
        [Required(ErrorMessage = "Campo SellerId de preenchimento obrigatório")]
        public int? SellerId { get; set; }

        public DateTime? SaleDate { get; set; }
    }
}
