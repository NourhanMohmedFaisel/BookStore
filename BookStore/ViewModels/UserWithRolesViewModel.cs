using BookStore.Models;

namespace BookStore.ViewModels
{
    public class UserWithRolesViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
