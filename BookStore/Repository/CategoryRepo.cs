using BookStore.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class CategoryRepo:ICategoryRepo
    {
        Context context;

        public CategoryRepo(Context _context)
        {
            context = _context;
        }
        public void Add(Category category)
        {
            context.Add(category);
        }

        public void Update(Category category)
        {
            context.Update(category);
        }
        public void Delete(int id)
        {
            Category category = GetById(id);
            if (category != null)
            {
                context.Categories.Remove(category);
            }
           
        }

        public List<Category> GetAll()
        {
            return context.Categories
        .Include(c => c.Books.Where(b => !b.IsDeleted)) 
        .Where(c => !c.IsDeleted)
        .ToList();
        }

        public Category GetById(int id)
        {
            return context.Categories.FirstOrDefault(b => b.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
