using BookStore.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class WishlistItemRepo:IWishlistItemRepo
    {
        Context context;
        public WishlistItemRepo(Context _context)
        {
            context = _context;
        }
        public void Add(WishlistItem item)
        {
            context.WishlistItems.Add(item);
        }

        public void Remove(WishlistItem item)
        {
            context.WishlistItems.Remove(item);
        }

        public WishlistItem GetById(int id)
        {
            return context.WishlistItems.Find(id);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
