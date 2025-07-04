using BookStore.Models;

namespace BookStore.IRepository
{
    public interface IOrderRepo
    {
        public Order GetById(int id);
        public List<Order> GetOrdersByUserId(int userId);
        public void CreateOrder(Order order);
        public void UpdateOrder(Order order);
        public void DeleteOrder(Order order);
        public int GetOrderCount();

        public double GetTotalRevenue();
        public void Save();
    }
}
