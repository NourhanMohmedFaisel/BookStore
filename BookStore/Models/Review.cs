namespace BookStore.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; } 
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

       
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
