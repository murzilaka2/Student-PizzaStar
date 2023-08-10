using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaStar.Interfaces;
using PizzaStar.Models;
using PizzaStar.Models.Checkout;
using PizzaStar.Models.Pages;
using PizzaStar.Repository;
using PizzaStar.ViewModels;
using System.Data;
using System.Security.Claims;

namespace PizzaStar.Controllers
{
    public class OrderController : Controller
    {
        private readonly CartRepository _cartRepository;
        private readonly IOrder _orders;
        private readonly UserManager<User> _userManager;

        public OrderController(CartRepository cartRepository, IOrder orders, UserManager<User> userManager)
        {
            _cartRepository = cartRepository;
            _orders = orders;
            _userManager = userManager;
        }

        [Route("order-info")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return NotFound();
                }
                var currentUser = await _userManager.FindByIdAsync(userId);
                return View("Authenticated", new OrderViewModelAuthenticated
                {
                    City = currentUser.City,
                    Address = currentUser.Address
                });
            }
            else
            {
                return View("NonAuthenticated");
            }
        }
        [Route("order-finish-result")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> FinishOrderNonAuthenticated(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order()
                {
                    Fio = orderViewModel.Fio,
                    Email = orderViewModel.Email,
                    Phone = orderViewModel.Phone,
                    City = orderViewModel.City,
                    Address = orderViewModel.Address,
                };
                var products = await _cartRepository.GetShopCartItemsAsync();
                var orderDetails = products.Select(e => new OrderDetails
                {
                    Order = order,
                    ProductId = e.ProductId,
                    Quantity = e.Count
                }).ToList();
                order.OrderDetails = orderDetails;
                await _orders.AddOrderAsync(order);
                await _cartRepository.ClearCartAsync();
                return View("ThankYou");
            }
            return View("NonAuthenticated", orderViewModel);
        }
        [Route("order-finish")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> FinishOrder(OrderViewModelAuthenticated orderViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return NotFound();
                }
                var currentUser = await _userManager.FindByIdAsync(userId);
                Order order = new Order()
                {
                    UserId = userId,
                    City = orderViewModel.City,
                    Address = orderViewModel.Address,
                };
                if (!currentUser.City.Equals(orderViewModel.City, StringComparison.OrdinalIgnoreCase))
                {
                    currentUser.City = orderViewModel.City;
                    var result = await _userManager.UpdateAsync(currentUser);
                }
                if (!currentUser.Address.Equals(orderViewModel.Address, StringComparison.OrdinalIgnoreCase))
                {
                    currentUser.Address = orderViewModel.Address;
                    var result = await _userManager.UpdateAsync(currentUser);
                }
                var products = await _cartRepository.GetShopCartItemsAsync();
                var orderDetails = products.Select(e => new OrderDetails
                {
                    Order = order,
                    ProductId = e.ProductId,
                    Quantity = e.Count
                }).ToList();
                order.OrderDetails = orderDetails;
                await _orders.AddOrderAsync(order);
                await _cartRepository.ClearCartAsync();
                return View("ThankYou");
            }
            return View("Authenticated", orderViewModel);
        }
        [Route("orders")]
        [HttpGet]
        public IActionResult MyOrders(QueryOptions options)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            return View(_orders.GetAllOrdersByUserWithDetails(options, userId));
        }

        [Authorize(Roles = "Admin")]
        [Route("/panel-orders")]
        [HttpGet]
        public IActionResult Orders(QueryOptions options)
        {
            return View(_orders.GetAllOrdersWithDetails(options));
        }

        [Authorize(Roles = "Admin")]
        [Route("/panel/delete-order")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var currentOrder = await _orders.GetOrderAsync(orderId);
            if (currentOrder != null)
            {
                await _orders.RemoveOrderAsync(currentOrder);
            }
            return Ok();
        }
    }
}
