using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistRepo _wishlistRepository;
        private readonly IBookRepo _bookRepository;

        public WishlistController(IWishlistRepo wishlistRepository, IBookRepo bookRepository)
        {
            _wishlistRepository = wishlistRepository;
            _bookRepository = bookRepository;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); 
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var wishlist = _wishlistRepository.GetWishlistByUserId(currentUserId);

            if (wishlist == null || wishlist.WishlistItems == null || !wishlist.WishlistItems.Any())
            {
                ViewBag.Message = "Wishlist is empty";
                return View(new List<Book>());
            }

            var books = wishlist.WishlistItems
                .Where(wi => wi.Book != null && !wi.Book.IsDeleted)
                .Select(wi => wi.Book)
                 .ToList();

            return View(books);
        }


    }
}
