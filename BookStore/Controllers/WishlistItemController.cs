using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class WishlistItemController : Controller
    {
        private readonly IWishlistItemRepo _wishlistItemRepository;
        private readonly IWishlistRepo _wishlistRepository;

        public WishlistItemController(IWishlistItemRepo wishlistItemRepository, IWishlistRepo wishlistRepository)
        {
            _wishlistItemRepository = wishlistItemRepository;
            _wishlistRepository = wishlistRepository;
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle([FromBody] int bookId)
        {
           

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var wishlist = _wishlistRepository.GetWishlistByUserId(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    ApplicationUserId = userId,
                    WishlistItems = new List<WishlistItem>()
                };
                _wishlistRepository.Create(wishlist);
                _wishlistRepository.Save();
            }

            var existingItem = wishlist.WishlistItems.FirstOrDefault(w => w.BookId == bookId);

            if (existingItem != null)
            {
                _wishlistItemRepository.Remove(existingItem);
                _wishlistItemRepository.Save();
                return Json(new { status = "removed" });
            }
            else
            {
                var wishlistItem = new WishlistItem
                {
                    WishlistId = wishlist.Id,
                    BookId = bookId
                };
                _wishlistItemRepository.Add(wishlistItem);
                _wishlistItemRepository.Save();
                return Json(new { status = "added" });
            }
        }



    }
}
