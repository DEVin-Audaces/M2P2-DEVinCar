using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Dtos {
    public class UserDto {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(150)]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}