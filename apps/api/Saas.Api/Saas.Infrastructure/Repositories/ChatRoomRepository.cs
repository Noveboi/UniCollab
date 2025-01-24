using Microsoft.EntityFrameworkCore;
using Saas.Application.Interfaces.Data;
using Saas.Domain;

namespace Saas.Infrastructure.Repositories;

internal class ChatRoomRepository(UniCollabContext context) : IChatRoomRepository
{
    public async Task<List<ChatRoom>> GetAllAsync()
    {
        return await context.ChatRooms.ToListAsync();
    }  

    public async Task<ChatRoom?> GetByIdAsync(Guid chatRoomId)
    {
        var chatRoom = await context.ChatRooms
            .Include(c => c.Participants)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(u => u.Id == chatRoomId);
            
        return chatRoom;
    }

    public Task AddAsync(ChatRoom room)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}