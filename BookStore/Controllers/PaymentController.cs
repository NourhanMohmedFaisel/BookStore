using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IOrderRepo _orderRepository;
        private readonly IPaymentRepo _paymentRepository;
       
        public PaymentController(IOrderRepo orderRepository, IPaymentRepo paymentRepository)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
        }
        public IActionResult Checkout(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null || order.ApplicationUserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public IActionResult Process(int orderId, string paymentMethod)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null || order.ApplicationUserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return NotFound();
            }

            var payment = new Payment
            {
                OrderId = orderId,
                PaymentMethod = paymentMethod,
                PaymentDate = DateTime.Now,
                AmountPaid = order.TotalAmount,
                IsSuccessful = true 
            };

            _paymentRepository.Add(payment);
            _paymentRepository.Save();

            order.Status = "Paid";
            _orderRepository.Save();

            return RedirectToAction("Confirmation", new { orderId = order.Id });
        }

        public IActionResult Confirmation(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null || order.ApplicationUserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
