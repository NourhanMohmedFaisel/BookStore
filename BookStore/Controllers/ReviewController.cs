using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepo _reviewRepository;
        private readonly IBookRepo _bookRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(IReviewRepo reviewRepository, IBookRepo bookRepository, UserManager<ApplicationUser> userManager)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add(int bookId)
        {
            var book = _bookRepository.GetById(bookId);
            if (book == null)
                return NotFound();

            var userId = int.Parse(_userManager.GetUserId(User));

            var existingReview = _reviewRepository.GetReviewByUserAndBook(userId, bookId);
            if (existingReview != null)
            {
               
                return RedirectToAction("Edit", new { id = existingReview.Id });
            }

            var model = new ReviewViewModel
            {
                BookId = bookId,
                BookTitle = book.Title,
                ReviewDate = DateTime.Now
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Add(ReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var userId = int.Parse(_userManager.GetUserId(User));

            var review = new Review
            {
                Rating = model.Rating,
                Comment = model.Comment,
                ReviewDate = DateTime.Now,
                ApplicationUserId = userId,
                BookId = model.BookId
            };

            _reviewRepository.Add(review);
            _reviewRepository.Save();

            return RedirectToAction("Details", "Book", new { id = model.BookId });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var review = _reviewRepository.GetByIdWithBookAndUser(id);
            if (review == null)
                return NotFound();

            var currentUserId = int.Parse(_userManager.GetUserId(User));
            if (review.ApplicationUserId != currentUserId)
                return Forbid();

            var vm = new ReviewViewModelWithId
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                BookId = review.BookId,
                BookTitle = review.Book?.Title,
                ReviewDate = review.ReviewDate,
                UserName = review.ApplicationUser?.UserName
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReviewViewModelWithId model)
        {
            if (!ModelState.IsValid)
            {
                var book = _bookRepository.GetById(model.BookId); 
                model.BookTitle = book?.Title;
                return View(model);
            }

            var review = _reviewRepository.GetById(model.Id);
            if (review == null)
                return NotFound();

            var currentUserId = int.Parse(_userManager.GetUserId(User));
            if (review.ApplicationUserId != currentUserId)
                return Forbid();

            review.Rating = model.Rating;
            review.Comment = model.Comment;
            review.ReviewDate = DateTime.Now;

            _reviewRepository.Update(review);
            _reviewRepository.Save();

            TempData["SuccessMessage"] = "Review updated successfully!";
            return RedirectToAction("Details", "Book", new { id = model.BookId });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var review = _reviewRepository.GetByIdWithBookAndUser(id); 
            if (review == null)
                return NotFound();

            var currentUserId = int.Parse(_userManager.GetUserId(User));
            if (review.ApplicationUserId != currentUserId)
                return Forbid();

            var model = new ReviewViewModelWithId
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                BookId = review.BookId,
                ReviewDate = review.ReviewDate,
                BookTitle = review.Book?.Title,
                UserName = review.ApplicationUser?.UserName
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = _reviewRepository.GetById(id);
            if (review == null)
                return NotFound();

            var currentUserId = int.Parse(_userManager.GetUserId(User));
            if (review.ApplicationUserId != currentUserId)
                return Forbid();

            _reviewRepository.Delete(id);
            _reviewRepository.Save();

            TempData["SuccessMessage"] = "Review deleted successfully!";
            return RedirectToAction("Details", "Book", new { id = review.BookId });
        }







    }
}
