using BookStore.Models;

namespace BookStore.Repository
{
    public interface IBookRepo
    {
        public void Add(Book obj);

        public void Update(Book obj);
       public int GetBooksCount();

        public void Delete(int id);

        public List<Book> GetAll();

        public Book GetById(int id);
        public List<Book> GetBestSellingBooks(int count);

        public List<Book> Search(string keyword);
        public void Save();
    }
}
