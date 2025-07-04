namespace BookStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public bool IsDeleted { get; set; }

      
        public List<Book> Books { get; set; }
    }
}
