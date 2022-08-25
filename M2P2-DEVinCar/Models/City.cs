using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M2P2_DEVinCar.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Name de preenchimento obrigatório")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo StateId de preenchimento obrigatório")]
        [ForeignKey("State")]
        public int StateId { get; set; }

        public State? State { get; set; }
    }
}
