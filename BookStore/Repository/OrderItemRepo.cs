using BookStore.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class OrderItemRepo:IOrderItemRepo
    {
        Context context;
        public OrderItemRepo(Context _context)
        {
            context = _context;
        }
        public OrderItem GetById(int id)
        {
            return context.OrderItems
                .Include(oi => oi.Book)
                .FirstOrDefault(oi => oi.Id == id);
        }

        public List<OrderItem> GetByOrderId(int orderId)
        {
            return context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Book)
                .ToList();
        }

        public void Add(OrderItem orderItem)
        {
            context.OrderItems.Add(orderItem);
        }

        public void Update(OrderItem orderItem)
        {
            context.OrderItems.Update(orderItem);
        }

        public void Delete(OrderItem orderItem)
        {
            context.OrderItems.Remove(orderItem);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
