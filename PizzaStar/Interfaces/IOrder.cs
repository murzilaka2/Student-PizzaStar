using PizzaStar.Models;
using PizzaStar.Models.Cart;
using PizzaStar.Models.Checkout;
using PizzaStar.Models.Pages;

namespace PizzaStar.Interfaces
{
    public interface IOrder
    {
        PagedList<Order> GetAllOrdersWithDetails(QueryOptions options);
        PagedList<Order> GetAllOrdersByUserWithDetails(QueryOptions options, string userId);
        Task<Order> GetOrderAsync(int id);

        Task AddOrderAsync(Order order);
        Task EditOrderAsync(Order order);
        Task RemoveOrderAsync(Order order);
    }
}
