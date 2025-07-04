using BookStore.Models;

namespace BookStore.IRepository
{
    public interface IWishlistRepo
    {
       public Wishlist GetWishlistByUserId(int userId);
       public void Create(Wishlist wishlist);
       public void Save();
    }
}
