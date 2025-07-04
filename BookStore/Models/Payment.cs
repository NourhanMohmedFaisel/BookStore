namespace BookStore.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; } 
        public DateTime PaymentDate { get; set; }
      
        public bool IsSuccessful { get; set; }
        public double AmountPaid { get; set; }

       
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
