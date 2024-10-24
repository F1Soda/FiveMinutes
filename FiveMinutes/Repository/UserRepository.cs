﻿using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Repository
{
    public class UserRepository : DefaultRepository<AppUser> ,IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
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
            return await context.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            context.Update(user);
            return Save();
        }
    }
}
