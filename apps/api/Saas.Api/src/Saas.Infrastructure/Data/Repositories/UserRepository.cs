﻿using Microsoft.EntityFrameworkCore;
using Saas.Application.Interfaces.Data;
using Saas.Domain;

namespace Saas.Infrastructure.Data.Repositories;

internal class UserRepository(UniCollabContext context) : IUserRepository
{
    public async Task<List<User>> GetAllAsync()
    {
        return await context.Users
            .AsSplitQuery()
            .Include(u => u.Posts)
            .Include(u => u.Friends)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid userId) =>
        await context.Users
            .Include(u => u.Friends)
            .Include(u => u.Posts)
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<List<User>> GetByIdsAsync(List<Guid> userIds) =>
        await context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

    public async Task<User?> GetByBasicCredentialsAsync(string username, string password) =>
        await context.Users.FirstOrDefaultAsync(u => u.Username == username &&
                                                     u.Password == password);

    public async Task<User?> GetByGoogleCredentialsAsync(string email, string googleId) =>
        await context.Users.FirstOrDefaultAsync(u => u.Username == email &&
                                                     u.GoogleId == googleId);

    public async Task<User?> GetByUsernameAsync(string username) => 
        await context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<bool> GoogleIdExistsAsync(string googleId) =>
        await context.Users.AnyAsync(u => u.GoogleId == googleId);


    public void Add(User user) => context.Users.Add(user);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}