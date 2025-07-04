using BookStore.Models;

namespace BookStore.IRepository
{
    public interface IReviewRepo
    {
        public List<Review> GetReviewsByBookId(int bookId);
        public void Add(Review review);
        public void Update(Review review);
        public Review GetReviewByUserAndBook(int userId, int bookId);
        public void Delete(int id);
        public Review GetByIdWithBookAndUser(int id);
        public Review GetById(int id);
        public void Save();
    }
}
