using Microsoft.AspNetCore.Mvc;
using PizzaStar.Interfaces;

namespace PizzaStar.ViewComponents
{
    public class RelatedProductsViewComponent : ViewComponent
    {
        private readonly IProduct _products;

        public RelatedProductsViewComponent(IProduct products)
        {
            _products = products;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            return View("RelatedProducts", await _products.GetEightRandomProductsAsync(productId));
        }
    }
}
