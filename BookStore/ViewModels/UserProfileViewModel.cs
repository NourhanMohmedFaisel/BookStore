using BookStore.Models;

namespace BookStore.ViewModels
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
