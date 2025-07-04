
using BookStore.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository

{
    public class CartRepo:ICartRepo

    {
        Context context;
        public CartRepo(Context _context)
        {
            context = _context;
        }
        public Cart GetCartByUserId(int userId)
        {
            return context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book)
                .FirstOrDefault(c => c.ApplicationUserId == userId);
        }

        public void Create(Cart cart)
        {
            context.Carts.Add(cart);
        }

        public void Update(Cart cart)
        {
            context.Carts.Update(cart);
        }

        public void Delete(Cart cart)
        {
            context.Carts.Remove(cart);
        }
        public void ClearCart(int userId)
        {
            var cart = context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.ApplicationUserId == userId);

            if (cart != null && cart.CartItems.Any())
            {
                context.CartItems.RemoveRange(cart.CartItems);
                context.SaveChanges();
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
