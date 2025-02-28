﻿using Microsoft.EntityFrameworkCore;
using Saas.Application.Interfaces.Data;
using Saas.Domain;

namespace Saas.Infrastructure.Data.Repositories;

internal sealed class PostRepository(UniCollabContext context) : IPostRepository
{
    public async Task<Post?> GetBySlugAsync(string slug)
    {
        var post = await context.Posts
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Slug == slug);
        
        return post;
    }

    public async Task<List<Post>> GetMostRecentAsync(int count)
    {
        var posts = await context.Posts
            .OrderByDescending(post => post.CreatedAt)
            .Take(count)
            .AsSplitQuery()
            .ToListAsync();

        return posts;
    }

    public async Task<List<Post>> GetByUserAsync(Guid userId)
    {
        var posts = await context.Posts
            .Where(p => p.Author.Id == userId)
            .ToListAsync();

        return posts;
    }

    public void Add(Post post) => context.Posts.Add(post);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}