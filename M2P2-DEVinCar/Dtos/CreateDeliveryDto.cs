using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Dtos
{
    public class CreateDeliveryDto
    {
        [Required(ErrorMessage = "Campo AddressId de preenchimento obrigatório")]
        public int? AddressId { get; set; }
        public DateTime? DeliveryForecast { get; set; }

    }
}
