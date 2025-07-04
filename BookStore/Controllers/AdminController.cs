using BookStore.IRepository;
using BookStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBookRepo _bookRepository;
        private readonly IOrderRepo _orderRepository;
        private readonly IApplicationUserRepo _applicationUserRepository;
        public AdminController(IBookRepo bookRepository, IOrderRepo orderRepository, IApplicationUserRepo applicationUserRepository)
        {
            _bookRepository = bookRepository;
            _orderRepository = orderRepository;
            _applicationUserRepository = applicationUserRepository;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            ViewBag.BookCount = _bookRepository.GetBooksCount();
            ViewBag.OrderCount = _orderRepository.GetOrderCount();
            ViewBag.TotalRevenue = _orderRepository.GetTotalRevenue();
            ViewBag.UserCount = _applicationUserRepository.GetUserCount();
            return View();
        }
    }
}
