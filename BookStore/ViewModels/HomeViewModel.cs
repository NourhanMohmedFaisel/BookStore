using BookStore.Models;

namespace BookStore.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Book> Books { get; set; }

        public List<Book> BestSellingBooks { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int BestSellersCurrentPage { get; set; }
        public int BestSellersTotalPages { get; set; }

        public int CategoriesCurrentPage { get; set; }
        public int CategoriesTotalPages { get; set; }
    }
}
