using Microsoft.AspNetCore.Mvc;
using PizzaStar.Interfaces;
using PizzaStar.Repository;

namespace PizzaStar.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IProduct _products;
        private readonly CartRepository _cartRepository;

        public ShopCartController(IProduct products, CartRepository cartRepository)
        {
            _products = products;
            _cartRepository = cartRepository;
        }

        [Route("/cart")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Что бы не была установлена активной главная страница.
            ViewBag.CategoryId = double.NaN;
            var products = await _cartRepository.GetShopCartItemsAsync();
            _cartRepository.ShopCartItems = products.ToList();
            return View(products);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddToCart(int productId, string? returnUrl, int quantity = 1, bool isModal = true)
        {
            var product = await _products.GetProductAsync(productId);
            if (product != null)
            {
                await _cartRepository.AddToCartAsync(product, quantity);
                if (isModal)
                {
                    return PartialView("_ConfirmModal", (product.Name, quantity));
                }
            }
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> IncrementQuantity(int shopCartItemId)
        {
            var currentShopCartItem = await _cartRepository.GetShopCartItemAsync(shopCartItemId);
            if (currentShopCartItem != null)
            {
                currentShopCartItem.Count += 1;
                await _cartRepository.UpdateFromCartAsync(currentShopCartItem);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DecrementQuantity(int shopCartItemId)
        {
            var currentShopCartItem = await _cartRepository.GetShopCartItemAsync(shopCartItemId);
            if (currentShopCartItem.Count > 1)
            {
                if (currentShopCartItem != null)
                {
                    currentShopCartItem.Count -= 1;
                    await _cartRepository.UpdateFromCartAsync(currentShopCartItem);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveFromCart(int shopCartItemId)
        {
            var currentShopCartItem = await _cartRepository.GetShopCartItemAsync(shopCartItemId);
            if (currentShopCartItem != null)
            {
                await _cartRepository.RemoveFromCartAsync(currentShopCartItem);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
