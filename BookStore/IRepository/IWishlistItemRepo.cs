using BookStore.Models;

namespace BookStore.IRepository
{
    public interface IWishlistItemRepo
    {
       public void Add(WishlistItem item);
       public void Remove(WishlistItem item);
        public WishlistItem GetById(int id);
        public void Save();
    }
}
