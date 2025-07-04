using BookStore.Models;

namespace BookStore.IRepository
{
    public interface IOrderItemRepo
    {
        OrderItem GetById(int id);
        List<OrderItem> GetByOrderId(int orderId);
        void Add(OrderItem orderItem);
        void Update(OrderItem orderItem);
        void Delete(OrderItem orderItem);
        void Save();
    }
}
