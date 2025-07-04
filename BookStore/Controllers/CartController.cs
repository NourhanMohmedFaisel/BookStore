using BookStore.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepo _cartRepository;
        private readonly ICartItemRepo _cartItemRepository;

        public CartController(ICartRepo cartRepository, ICartItemRepo cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var cart = _cartRepository.GetCartByUserId(userId);
            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                ViewBag.Message = "Cart is Empty";
                return View(new List<CartItem>());
            }

            return View(cart.CartItems);
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _cartRepository.ClearCart(userId);

            return RedirectToAction("Index");
        }

    }
}
