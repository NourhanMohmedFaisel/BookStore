using BookStore.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class ApplicationUserRepo:IApplicationUserRepo
    {

        Context context;
        private readonly UserManager<ApplicationUser> userManager;
        public ApplicationUserRepo(Context _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }
        public int GetUserCount()
        {
            return context.ApplicationUsers.Count();
        }
        public List<ApplicationUser> GetAll()
        {
            return context.ApplicationUsers.ToList();
        }

        public ApplicationUser GetById(int id)
        {
            return context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
        }
        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return (await userManager.GetRolesAsync(user)).ToList();
        }
        public void Update(ApplicationUser user)
        {
            context.Update(user);
        }

        public void Save()
        {
            context.SaveChanges();
        }

    }
}
