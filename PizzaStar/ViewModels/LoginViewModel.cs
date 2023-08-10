using System.ComponentModel.DataAnnotations;

namespace PizzaStar.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите емейл")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Не корректный емейл")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }

}
