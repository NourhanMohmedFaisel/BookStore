using BookStore.Models;

namespace BookStore.ViewModels
{
    public class UserOrdersViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<Order> Orders { get; set; }
    }
}
