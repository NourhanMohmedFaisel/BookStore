using BookStore.Models;

namespace BookStore.IRepository
{
    public interface ICategoryRepo
    {
        public void Add(Category obj);

        public void Update(Category obj);


        public void Delete(int id);

        public List<Category> GetAll();

        public Category GetById(int id);

        public void Save();
    }
}
