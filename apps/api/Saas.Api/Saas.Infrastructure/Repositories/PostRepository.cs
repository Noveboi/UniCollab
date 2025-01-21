﻿using Microsoft.EntityFrameworkCore;
using Saas.Application.Interfaces.Data;
using Saas.Domain.Posts;

namespace Saas.Infrastructure.Repositories;

internal sealed class PostRepository(UniCollabContext context) : IPostRepository
{
    public async Task<Post?> GetBySlugAsync(string slug)
    {
        var post = await context.Posts
            .AsSplitQuery()
            .Include(p => p.Subjects)
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Slug == slug);
        
        return post;
    }

    public async Task<List<Post>> GetMostRecentAsync(int count)
    {
        var posts = await context.Posts
            .OrderBy(post => post.CreatedAt)
            .Take(count)
            .ToListAsync();

        return posts;
    }
    
    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}