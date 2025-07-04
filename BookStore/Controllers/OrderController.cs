using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartRepo _cartRepository;
        private readonly ICartItemRepo _cartItemRepository;
        private readonly IOrderRepo _orderRepository;
        private readonly IOrderItemRepo _orderItemRepository;

        public OrderController(ICartRepo cartRepository,
            ICartItemRepo cartItemRepository,
            IOrderRepo orderRepository,
            IOrderItemRepo orderItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cart = _cartRepository.GetCartByUserId(userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            return View(cart.CartItems);
        }


        [HttpPost]
        public IActionResult PlaceOrder()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var cart = _cartRepository.GetCartByUserId(userId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            double totalAfterDiscount = 0;

            foreach (var item in cart.CartItems)
            {
                double originalPrice = item.Book.Price;
                double discount = item.Book.Discount ?? 0;
                double finalPrice = originalPrice - discount;

                totalAfterDiscount += finalPrice * item.Quantity;

                var orderItem = new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = finalPrice
                };

                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = totalAfterDiscount;

            _orderRepository.CreateOrder(order);
            _orderRepository.Save();

            _cartItemRepository.ClearCart(cart.Id);
            _cartItemRepository.Save();

            return RedirectToAction("Checkout", "Payment", new { orderId = order.Id });
        }




    }
}
