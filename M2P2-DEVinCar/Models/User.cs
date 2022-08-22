﻿using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo nome é obrigatório.")]
        [StringLength(255, ErrorMessage = "O tamanho máximo do campo nome é de 255 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [StringLength(150, ErrorMessage = "O tamanho máximo do campo nome é de 255 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "O tamanho máximo da senha deve ser de 50 carateres")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]    
        public DateTime BirthDate { get; set; }
    }
}
