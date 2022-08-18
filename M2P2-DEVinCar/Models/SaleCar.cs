using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Models
{
    public class SaleCar
    {
    
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo SaleId de preenchimento obrigatório")]
        [ForeignKey("Sale")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "Campo CarId de preenchimento obrigatório")]
        [ForeignKey("Car")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Campo UnitPrice de preenchimento obrigatório")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Campo Amount de preenchimento obrigatório")]
        public int Amount { get; set; }

        public Sale? Sale { get; set; }

        public Car? Car { get; set; }
    }
}