using PizzaStar.Models;

namespace PizzaStar.ViewModels
{
    public class CurrentProductViewModel
    {
        public Product Product { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
