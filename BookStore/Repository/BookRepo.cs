using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class BookRepo:IBookRepo
    {
        Context context;
        public BookRepo(Context _context)
        {
            context = _context;
        }
        public void Add(Book obj)
        {
            context.Add(obj);
        }

        public void Update(Book obj)
        {
            context.Update(obj);
        }

        public int GetBooksCount()
        {
            return context.Books.Count(b => !b.IsDeleted);
        }
        public void Delete(int id)
        {
            Book book=GetById(id);
            context.Books.Remove(book);
        }

        public List<Book> GetAll()
        {
           
            return context.Books.Include(b => b.Category).Where(b => !b.IsDeleted).ToList();
        }

        public Book GetById(int id)
        {
            return context.Books
         .Include(b => b.Reviews)
             .ThenInclude(r => r.ApplicationUser)
         .FirstOrDefault(b => b.Id == id && !b.IsDeleted); 
        }
        public List<Book> Search(string keyword)
        {
            return context.Books
                .Include(b => b.Category)
                .Where(b => !b.IsDeleted && (
                            b.Title.Contains(keyword) ||
                            b.Author.Contains(keyword)))
                .ToList();
        }

        public List<Book> GetBestSellingBooks(int count)
        {
            return context.OrderItems
                .Where(oi => oi.Book != null && !oi.Book.IsDeleted) 
                .GroupBy(oi => oi.BookId)
                .OrderByDescending(g => g.Sum(oi => oi.Quantity))
                .Select(g => g.First().Book)
                .Take(count)
                .ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
