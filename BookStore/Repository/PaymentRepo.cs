using BookStore.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class PaymentRepo:IPaymentRepo
    {
        Context context;
        public PaymentRepo(Context _context)
        {
            context = _context;
        }
        public void Add(Payment payment)
        {
            context.Payments.Add(payment);
        }

        public Payment GetById(int id)
        {
            return context.Payments
                .Include(p => p.Order)
                .FirstOrDefault(p => p.Id == id);
        }

        public Payment GetByOrderId(int orderId)
        {
            return context.Payments
                .Include(p => p.Order)
                .FirstOrDefault(p => p.OrderId == orderId);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
