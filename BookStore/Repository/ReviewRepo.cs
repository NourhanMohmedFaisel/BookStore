using BookStore.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class ReviewRepo:IReviewRepo
    {
        Context context;
        public ReviewRepo(Context _context)
        {
            context = _context;
        }
        public List<Review> GetReviewsByBookId(int bookId)
        {
            return context.Reviews
                .Where(r => r.BookId == bookId)
                .Include(r => r.ApplicationUser)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();
        }
        public Review GetReviewByUserAndBook(int userId, int bookId)
        {
            return context.Reviews.FirstOrDefault(r => r.ApplicationUserId == userId && r.BookId == bookId);
        }


        public void Add(Review review)
        {
            context.Reviews.Add(review);
        }
        public void Update(Review review)
        {
            context.Reviews.Update(review);
        }


        public void Delete(int id)
        {
            var review = GetById(id);
            if (review != null)
            {
                context.Reviews.Remove(review);
            }
        }

        public Review GetById(int id)
        {
            return context.Reviews.FirstOrDefault(r => r.Id == id);
        }
        public Review GetByIdWithBookAndUser(int id)
        {
            return context.Reviews
                .Include(r => r.Book)
                .Include(r => r.ApplicationUser)
                .FirstOrDefault(r => r.Id == id);
        }


        public void Save()
        {
            context.SaveChanges();
        }
    }
}
