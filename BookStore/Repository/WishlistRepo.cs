using BookStore.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class WishlistRepo:IWishlistRepo
    {
        Context context;
        public WishlistRepo(Context _context)
        {
            context = _context;
        }
        public Wishlist GetWishlistByUserId(int userId)
        {
            return context.Wishlists.Include(w => w.WishlistItems)
                                     .ThenInclude(i => i.Book)
                                     .FirstOrDefault(w => w.ApplicationUserId == userId);
        }

        public void Create(Wishlist wishlist)
        {
            context.Wishlists.Add(wishlist);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
