using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M2P2_DEVinCar.Models
{
    public class State
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Name de preenchimento obrigat�rio")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo Initials de preenchimento obrigat�rio")]
        [StringLength(2)]
        public string Initials { get; set; }

    }
} 