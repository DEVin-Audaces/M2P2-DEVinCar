using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVInCar.Models
{
    public class SaleCar
    {
    
        public int Id { get; set; }

        [Required(ErrorMessage = "SaleId is required.")]
        [ForeignKey("Sale")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "CarId is required.")]
        [ForeignKey("Car")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "UnitPrice is required.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public int Amount { get; set; }

        public Sale? Sale { get; set; }

        public Car? Car { get; set; }
    }
}