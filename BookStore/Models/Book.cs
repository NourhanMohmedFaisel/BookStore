namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }

        public String Description {  get; set; }
        public string ImagePath { get; set; } 

        public bool IsDeleted { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public double? Discount { get; set; } 

       
        public List<CartItem> CartItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<WishlistItem> WishlistItems { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
