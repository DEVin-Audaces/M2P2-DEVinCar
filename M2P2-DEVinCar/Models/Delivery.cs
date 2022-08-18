using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M2P2_DEVinCar.Models
{

    public class Delivery
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo AddressId de preenchimento obrigatório")]
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        [Required(ErrorMessage = "Campo SaleId de preenchimento obrigatório")]
        [ForeignKey("Sale")]
        public int SaleId { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "Formato de data inválido")]
        public DateTime? DeliveryForecast { get; set; }
        public Address? Address { get; set; }
        public Sale? Sale { get; set; }

    }
}