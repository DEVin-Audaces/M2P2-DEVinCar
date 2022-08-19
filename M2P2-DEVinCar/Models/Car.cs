using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M2P2_DEVinCar.Models {
    public class Car {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [Range(9, 100000000000000000, ErrorMessage = "O campo SuggestedPrice deve ser entre 9 e 100000000000000000.")]
        public double SuggestedPrice { get; set; }
    }
}
