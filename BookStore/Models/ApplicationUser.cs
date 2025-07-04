using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class ApplicationUser: IdentityUser<int>
    {
        public Wishlist Wishlist { get; set; }
        public Cart Cart { get; set; }
        public List<Order> Orders { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
