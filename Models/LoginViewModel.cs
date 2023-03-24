using System.ComponentModel.DataAnnotations;

namespace Evdoshenko_lab10.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
