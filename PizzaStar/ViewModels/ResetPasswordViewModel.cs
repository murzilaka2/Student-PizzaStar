using System.ComponentModel.DataAnnotations;

namespace PizzaStar.ViewModels
{
    public class ResetPasswordViewModel
    {
        [MinLength(5, ErrorMessage = "Минимальная длинна пароля 5 символов")]
        [Required(ErrorMessage = "Введите новый пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }

        [MinLength(5, ErrorMessage = "Минимальная длинна пароля 5 символов")]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }

}
