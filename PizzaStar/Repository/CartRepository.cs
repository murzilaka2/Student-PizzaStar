using Microsoft.EntityFrameworkCore;
using PizzaStar.Data;
using PizzaStar.Interfaces;
using PizzaStar.Models;
using PizzaStar.Models.Cart;
using System;

namespace PizzaStar.Repository
{
    public class CartRepository : ICart
    {
        private readonly ApplicationContext _applicationContext;

        public CartRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public string ShopCartId { get; set; }
        public List<ShopCartItem> ShopCartItems { get; set; } = new();

        public static CartRepository GetCart(IServiceProvider service)
        {
            //объект, с помощью которого, мы сможем работать с сессией
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            var context = service.GetService<ApplicationContext>();
            //получаем Id корзины пользователя из сессии, если значения нет, создаем новый идентификатор
            string shopCartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            //устаналиваем ID как сессию, если такого ID не было, создается новая сессия
            //если есть, то ничгео не меняется
            session.SetString("CartId", shopCartId);
            return new CartRepository(context) { ShopCartId = shopCartId };
        }

        public async Task AddToCartAsync(Product product, int quantity)
        {
            await _applicationContext.ShopCartItems.AddAsync(new ShopCartItem
            {
                ShopCartId = ShopCartId,
                ProductId = product.Id,
                Price = product.Price,
                Count = quantity
            });
            await _applicationContext.SaveChangesAsync();
        }
        public async Task<int> GetShopCartItemsCountAsync()
        {
            return await _applicationContext.ShopCartItems.Where(e => e.ShopCartId == ShopCartId).CountAsync();
        }
        public async Task<IEnumerable<ShopCartItem>> GetShopCartItemsAsync()
        {
            return await _applicationContext.ShopCartItems.Where(e => e.ShopCartId == ShopCartId).Include(e => e.Product).ToListAsync();
        }
        public async Task<ShopCartItem> GetShopCartItemAsync(int shopCartItemId)
        {
            return await _applicationContext.ShopCartItems.FirstOrDefaultAsync(e => e.Id == shopCartItemId);
        }
        public async Task RemoveFromCartAsync(ShopCartItem shopCartItem)
        {
            _applicationContext.ShopCartItems.Remove(shopCartItem);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task UpdateFromCartAsync(ShopCartItem shopCartItem)
        {
            _applicationContext.ShopCartItems.Update(shopCartItem);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task ClearCartAsync()
        {
            await _applicationContext.ShopCartItems.Where(e => e.ShopCartId == this.ShopCartId).ExecuteDeleteAsync();
        }
    }
}
