using Microsoft.EntityFrameworkCore;
using PizzaStar.Data;
using PizzaStar.Interfaces;
using PizzaStar.Models;
using PizzaStar.Models.Checkout;
using PizzaStar.Models.Pages;

namespace PizzaStar.Repository
{
    public class OrderRepository : IOrder
    {
        private readonly ApplicationContext _applicationContext;

        public OrderRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public async Task AddOrderAsync(Order order)
        {
            await _applicationContext.Orders.AddAsync(order);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task EditOrderAsync(Order order)
        {
            _applicationContext.Orders.Update(order);
            await _applicationContext.SaveChangesAsync();
        }

        public PagedList<Order> GetAllOrdersByUserWithDetails(QueryOptions options, string userId)
        {
            return new PagedList<Order>(_applicationContext.Orders.Include(e => e.OrderDetails).ThenInclude(e => e.Product)
                .Where(e => e.UserId.Equals(userId)).OrderByDescending(e => e.Id), options);
        }

        public PagedList<Order> GetAllOrdersWithDetails(QueryOptions options)
        {
            return new PagedList<Order>(_applicationContext.Orders.Include(e => e.OrderDetails).ThenInclude(e=>e.Product).Include(e=>e.User), options);
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _applicationContext.Orders.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task RemoveOrderAsync(Order order)
        {
            _applicationContext.Orders.Remove(order);
            await _applicationContext.SaveChangesAsync();
        }
    }
}
