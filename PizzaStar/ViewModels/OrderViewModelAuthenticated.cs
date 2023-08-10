using System.ComponentModel.DataAnnotations;

namespace PizzaStar.ViewModels
{
    public class OrderViewModelAuthenticated
    {
        [Required(ErrorMessage = "Введите название города")]
        [Display(Name = "Город")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Введите ваш адрес")]
        [Display(Name = "Адрес")]
        public string? Address { get; set; }
    }
}
