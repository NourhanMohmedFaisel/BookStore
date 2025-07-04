using BookStore.Models;

namespace BookStore.IRepository
{
    public interface ICartRepo
    {
        public Cart GetCartByUserId(int userId);
        public void Create(Cart cart);
        public void Update(Cart cart);
        public void Delete(Cart cart);
        public void ClearCart(int userId);
        public void Save();
    }
}
