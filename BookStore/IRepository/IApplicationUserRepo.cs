using BookStore.Models;

namespace BookStore.IRepository
{
    public interface IApplicationUserRepo
    {
        public int GetUserCount();
        List<ApplicationUser> GetAll();
        ApplicationUser GetById(int id);
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        public void Update(ApplicationUser user);
        public void Save();

    }
}
