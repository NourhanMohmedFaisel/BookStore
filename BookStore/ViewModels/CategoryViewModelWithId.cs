namespace BookStore.ViewModels
{
    public class CategoryViewModelWithId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImagePath { get; set; }

        public IFormFile? Image { get; set; }
    }
}
