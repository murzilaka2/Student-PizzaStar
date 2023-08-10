using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PizzaStar.ViewModels
{
    public class RequestForResetViewModel
    {
        [Required(ErrorMessage = "Укажите емейл адрес")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Не корректный емейл адрес")]
        [Remote(action: "IsEmailExists", controller: "Account", ErrorMessage = "Емейл адрес не найден")]
        public string Email { get; set; }
    }

}
