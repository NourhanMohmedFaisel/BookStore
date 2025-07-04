using BookStore.Models;

namespace BookStore.IRepository
{
    public interface ICartItemRepo
    {
        public CartItem GetById(int id);
        public List<CartItem> GetItemsByCartId(int cartId);
        public void Add(CartItem item);
        public void Update(CartItem item);
        public void Delete(CartItem item);
        public void Save();
        public void ClearCart(int cartId);
        public void AddItemToCart(int userId, int bookId);
        public void IncreaseQuantity(int cartItemId);
        public void DecreaseQuantity(int cartItemId);
        public void Remove(int cartItemId);

    }
}
