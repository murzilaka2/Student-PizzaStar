using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PizzaStar.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите емейл")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Не корректный емейл")]
        [Display(Name = "Email")]
        [Remote(action: "IsEmailInUse", controller: "Account", ErrorMessage = "Емейл адрес уже используется")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Введите год рождения")]
        [Display(Name = "Год рождения")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефона")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }
    }
}
