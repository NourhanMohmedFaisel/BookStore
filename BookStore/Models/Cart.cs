namespace BookStore.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

     
        public List<CartItem> CartItems { get; set; }
    }
}
