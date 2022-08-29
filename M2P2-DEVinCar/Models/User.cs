using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M2P2_DEVinCar.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo nome é obrigatório.")]
        [StringLength(255, ErrorMessage = "O tamanho máximo do campo nome é de 255 caracteres")]
        
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [DataType(DataType.EmailAddress)]
        [StringLength(150, ErrorMessage = "O tamanho máximo do campo nome é de 255 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "O tamanho máximo da senha deve ser de 50 carateres")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
    }
}
