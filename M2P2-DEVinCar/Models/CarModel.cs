using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M2P2_DEVinCar.Models {
    public class CarModel {
        [Required]
        public int Id { get; set; }
        [Required]
        [Column("Nome")]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [Column("PrecoSugerido")]
        [StringLength(18, MinimumLength = 2)]
        public double SuggestedPrice { get; set; }
    }
}
