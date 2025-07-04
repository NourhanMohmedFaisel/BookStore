using BookStore.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    
    public class OrderRepo:IOrderRepo
    {
        Context context;
        public OrderRepo(Context _context)
        {
            context = _context;
        }
        public Order GetById(int id)
        {
            return context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Include(o => o.Payment)
                .FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            return context.Orders
                .Where(o => o.ApplicationUserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                        .ThenInclude(b => b.Category) 
                .Include(o => o.Payment)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }


        public void CreateOrder(Order order)
        {
            context.Orders.Add(order);
        }

        public void UpdateOrder(Order order)
        {
            context.Orders.Update(order);
        }

        public void DeleteOrder(Order order)
        {
            context.Orders.Remove(order);
        }
        public int GetOrderCount()
        {
            return context.Orders.Count();
        }
        public double GetTotalRevenue()
        {
            return context.Orders.Sum(o => o.TotalAmount);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
