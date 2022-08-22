using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Dtos
{
    public class UpdateAddressDto
    {
        [StringLength(150)]
        public string? Street { get; set; }

        public int? Number { get; set; }

        [StringLength(255)]
        public string? Complement { get; set; }

        [StringLength(8)]
        public string? Cep { get; set; }
    }
}
