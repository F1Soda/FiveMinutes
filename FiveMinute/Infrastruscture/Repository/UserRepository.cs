using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FiveMinute.Repository
{
    public class UserRepository : DefaultRepository<AppUser> ,IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public Task<IdentityResult> CreateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }


        public Task<AppUser?> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await context.Users
                .Include(x => x.FMTemplates)
                .ThenInclude(x => x.Questions)
                .Include(x => x.FMTests)
                .Include(appUser => appUser.PassedTestResults)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<bool> AddFMTtoUser(FiveMinuteTemplate fmt,AppUser user)
        {
            user.AddFMT(fmt);
            return await Save();
        }
    }
}
