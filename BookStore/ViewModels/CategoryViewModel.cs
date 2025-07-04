using BookStore.Models;

namespace BookStore.ViewModels
{
    public class CategoryViewModel
    {
        public string Name { get; set; }
        public string? ImagePath { get; set; }

        public IFormFile? Image { get; set; }
    }
}
