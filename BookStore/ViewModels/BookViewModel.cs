using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.ViewModels
{
    public class BookViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; } 

        public IFormFile Image { get; set; }
        public int CategoryId { get; set; } 
        

        
        public List<Category>? Categories { get; set; }
    }
}
