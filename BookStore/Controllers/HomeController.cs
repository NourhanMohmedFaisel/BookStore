using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IBookRepo _bookRepository;
        private readonly ICategoryRepo _categoryRepository;
        public HomeController(ILogger<HomeController> logger , IBookRepo bookRepository,ICategoryRepo categoryRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(int page = 1, int bestSellersPage = 1, int categoriesPage = 1)
        {
            ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;
           

            int pageSize = 4;
            int bestSellersPageSize = 4;
            int categoriesPageSize = 3;

          
            var books = _bookRepository.GetAll();
            var totalBooks = books.Count();
            var pagedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

           
            var bestSellers = _bookRepository.GetBestSellingBooks(100); // Get all and then paginate
            var totalBestSellers = bestSellers.Count();
            var pagedBestSellers = bestSellers.Skip((bestSellersPage - 1) * bestSellersPageSize).Take(bestSellersPageSize).ToList();

            
            var categories = _categoryRepository.GetAll();
            var totalCategories = categories.Count();
            var pagedCategories = categories.Skip((categoriesPage - 1) * categoriesPageSize).Take(categoriesPageSize).ToList();

            var viewModel = new HomeViewModel
            {
                Books = pagedBooks,
                Categories = pagedCategories,
                BestSellingBooks = pagedBestSellers,

                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalBooks / pageSize),

                BestSellersCurrentPage = bestSellersPage,
                BestSellersTotalPages = (int)Math.Ceiling((double)totalBestSellers / bestSellersPageSize),

                CategoriesCurrentPage = categoriesPage,
                CategoriesTotalPages = (int)Math.Ceiling((double)totalCategories / categoriesPageSize)
            };

            return View(viewModel);
        }


        public IActionResult BestSellingBooks()
        {
            var bestSellingBooks = _bookRepository.GetBestSellingBooks(5); // ?????? ????? ???? ?????
            return View(bestSellingBooks);
        }


      
        public IActionResult Search(string title, int page = 1, int pageSize =4 )
        {
            ViewBag.IsAuthenticated = User.Identity.IsAuthenticated;

            var books = _bookRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(title))
            {
                title = title.Trim().ToLower();

                books = books.Where(b =>
                    (!string.IsNullOrEmpty(b.Title) && b.Title.ToLower().Contains(title)) ||
                    (!string.IsNullOrEmpty(b.Author) && b.Author.ToLower().Contains(title)) ||
                    (b.Category != null && !string.IsNullOrEmpty(b.Category.Name) && b.Category.Name.ToLower().Contains(title))
                ).ToList();
            }

           
            int totalItems = books.Count;
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var pagedBooks = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

           
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchQuery = title;

            return View("SearchResults", pagedBooks);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
