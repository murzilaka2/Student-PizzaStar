using PizzaStar.Models;
using PizzaStar.Models.Cart;
using System;

namespace PizzaStar.Interfaces
{
    public interface ICart
    {
        string ShopCartId { get; set; }
        List<ShopCartItem> ShopCartItems { get; set; }
        Task<int> GetShopCartItemsCountAsync();
        Task<IEnumerable<ShopCartItem>> GetShopCartItemsAsync();
        Task<ShopCartItem> GetShopCartItemAsync(int shopCartItemId);
        Task AddToCartAsync(Product product, int quantity);
        Task RemoveFromCartAsync(ShopCartItem shopCartItem);
        Task UpdateFromCartAsync(ShopCartItem shopCartItem);
        Task ClearCartAsync();
    }
}
