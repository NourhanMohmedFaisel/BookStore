using BookStore.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class CartItemRepo:ICartItemRepo
    {
        Context context;
        public CartItemRepo(Context _context)
        {
            context = _context;
        }
        public CartItem GetById(int id)
        {
            return context.CartItems
                .Include(ci => ci.Book)
                .FirstOrDefault(ci => ci.Id == id);
        }

        public List<CartItem> GetItemsByCartId(int cartId)
        {
            return context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Book)
                .ToList();
        }

        public void Add(CartItem item)
        {
            context.CartItems.Add(item);
        }

        public void Update(CartItem item)
        {
            context.CartItems.Update(item);
        }

        public void Delete(CartItem item)
        {
            context.CartItems.Remove(item);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void AddItemToCart(int userId, int bookId)
        {
            var cart = context.Carts.FirstOrDefault(c => c.ApplicationUserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId,
                    CartItems = new List<CartItem>()
                };
                context.Carts.Add(cart);
                context.SaveChanges();
            }

            var existingItem = context.CartItems
                .FirstOrDefault(ci => ci.CartId == cart.Id && ci.BookId == bookId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
                context.CartItems.Update(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    BookId = bookId,
                    Quantity = 1
                };
                context.CartItems.Add(newItem);
            }

            context.SaveChanges();
        }

     
        public void IncreaseQuantity(int cartItemId)
        {
            var item = context.CartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (item != null)
            {
                item.Quantity += 1;
                context.CartItems.Update(item);
                context.SaveChanges();
            }
        }

       
        public void DecreaseQuantity(int cartItemId)
        {
            var item = context.CartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (item != null)
            {
                item.Quantity -= 1;

                if (item.Quantity <= 0)
                {
                    context.CartItems.Remove(item);
                }
                else
                {
                    context.CartItems.Update(item);
                }

                context.SaveChanges();
            }
        }
        public void ClearCart(int cartId)
        {
            var items = context.CartItems.Where(i => i.CartId == cartId).ToList();
            context.CartItems.RemoveRange(items);
        }
        
        public void Remove(int cartItemId)
        {
            var item = context.CartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (item != null)
            {
                context.CartItems.Remove(item);
                context.SaveChanges();
            }
        }
    }
}
