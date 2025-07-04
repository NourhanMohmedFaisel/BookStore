namespace BookStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }

      
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        
        public List<OrderItem> OrderItems { get; set; }

      
        public Payment Payment { get; set; }
    }
}
