namespace BookStore.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

      
        public List<WishlistItem> WishlistItems { get; set; }
    }
}
