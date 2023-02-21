using System.ComponentModel.DataAnnotations;

namespace GADev.Chat.Api.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está com o formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(20, ErrorMessage = "O campo {0} dever ter o tamanho entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }
}