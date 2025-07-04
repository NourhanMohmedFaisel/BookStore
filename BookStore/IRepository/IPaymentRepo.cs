using BookStore.Models;

namespace BookStore.IRepository
{
    public interface IPaymentRepo
    {
        public void Add(Payment payment);
        public Payment GetById(int id);
        public Payment GetByOrderId(int orderId);
        public void Save();
    }
}
