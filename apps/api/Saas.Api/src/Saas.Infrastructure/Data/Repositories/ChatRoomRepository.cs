using Microsoft.EntityFrameworkCore;
using Saas.Application.Interfaces.Data;
using Saas.Domain;

namespace Saas.Infrastructure.Data.Repositories;

internal class ChatRoomRepository(UniCollabContext context) : IChatRoomRepository
{
    public async Task<List<ChatRoom>> GetJoinableFor(Guid userId)
    {
        return await context.ChatRooms
            .Include(c => c.Participants)
            .Where(c => c.Participants.All(p => p.Id != userId))
            .ToListAsync();
    }

    public async Task<List<ChatRoom>> GetByUserAsync(Guid userId)
    {
        // Retrieves the chat rooms and gets the last message sent.
        var chatRooms = await context.ChatRooms
            .Include(c => c.Participants)
            .Include(c => c.Messages
                .OrderByDescending(m => m.SentAt)
                .Take(1))
            .Where(c => c.Participants.Any(p => p.Id == userId))
            .ToListAsync();

        return chatRooms.OrderByDescending(m =>
        {
            return m.Messages == null || m.Messages.Count == 0 ? DateTime.MinValue : m.Messages.Max(r => r.SentAt);
        }).ToList();
    }

    public async Task<ChatRoom?> GetByIdAsync(Guid chatId)
    {
        var chatRoom = await context.ChatRooms
            .AsSplitQuery()
            .Include(c => c.Participants)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(u => u.Id == chatId);
            
        return chatRoom;
    }

    public async Task<IReadOnlyList<Message>?> GetMessagesAsync(Guid chatId)
    {
        var room = await context.ChatRooms
            .Include(c => c.Messages.OrderBy(m => m.SentAt))
            .FirstOrDefaultAsync(c => c.Id == chatId);

        return room?.Messages;
    }
    
    public async Task<List<ChatRoom>> GetMutualChatsOf(Guid userId1, Guid userId2)
    {
        return await context.ChatRooms
            .Include(c => c.Participants)
            .Where(c => c.Participants.Any(p => p.Id == userId1) &&
                        c.Participants.Any(p => p.Id == userId2))
            .ToListAsync();
    }

    public void Add(ChatRoom room) => context.ChatRooms.Add(room);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}