using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class CartItemController : Controller
    {
        private readonly ICartRepo _cartRepository;
        private readonly ICartItemRepo _cartItemRepository;

        public CartItemController(ICartRepo cartRepository, ICartItemRepo cartItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int bookId)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var cart = _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                cart = new Cart { ApplicationUserId = userId, CartItems = new List<CartItem>() };
                _cartRepository.Create(cart);
                _cartRepository.Save();
            }

            var existingItem = _cartItemRepository.GetItemsByCartId(cart.Id)
                                  .FirstOrDefault(i => i.BookId == bookId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
                _cartItemRepository.Update(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    BookId = bookId,
                    Quantity = 1,
                    CartId = cart.Id
                };
                _cartItemRepository.Add(newItem);
            }

            _cartItemRepository.Save();

            return RedirectToAction("Index", "Cart");
        }



        [HttpPost]
        public IActionResult IncreaseQuantity(int cartItemId)
        {
            var item = _cartItemRepository.GetById(cartItemId);
            if (item != null)
            {
                item.Quantity++;
                _cartItemRepository.Update(item);
                _cartItemRepository.Save();
            }
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int cartItemId)
        {
            var item = _cartItemRepository.GetById(cartItemId);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    _cartItemRepository.Update(item);
                }
                else
                {
                    _cartItemRepository.Delete(item);
                }

                _cartItemRepository.Save();
            }
            return RedirectToAction("Index", "Cart");
        }


        [HttpPost]
        public IActionResult Remove(int cartItemId)
        {
            var item = _cartItemRepository.GetById(cartItemId);
            if (item != null)
            {
                _cartItemRepository.Delete(item);
                _cartItemRepository.Save();
            }
            return RedirectToAction("Index", "Cart");
        }



    }
}
