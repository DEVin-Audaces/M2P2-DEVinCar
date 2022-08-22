using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M2P2_DEVinCar.Models {
    public class Car {

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }  
        
        [Required]
        public decimal SuggestedPrice { get; set; }
    }
}
