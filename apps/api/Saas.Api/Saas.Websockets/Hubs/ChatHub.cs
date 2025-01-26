﻿using Microsoft.AspNetCore.SignalR;
using Saas.Application.Common.Events;
using Saas.Application.Common.Notifications;
using Saas.Application.Interfaces;
using Saas.Websockets.Contracts;

namespace Saas.Websockets.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(ClientMessage message);
}

public record ServerMessage(string ChatId, string UserId, string Content);
public record ClientMessage(string Username, string Content);

public class ChatHub(IEventService eventService) : Hub<IChatClient>
{
    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
    
    public async Task SendMessage(ServerMessage message)
    {
        var username = Context.User?
            .FindFirst(c => c.Type.Equals("Username", StringComparison.OrdinalIgnoreCase))?.Value ?? Context.ConnectionId;

        var chatGuid = Guid.Parse(message.ChatId);
        var userGuid = Guid.Parse(message.UserId);
        
        var messageSentEvent = new ChatMessageSentEvent(
            SenderUsername: username,
            SenderId: userGuid,
            SenderConnectionId: Context.ConnectionId,
            ChatId: chatGuid,
            Message: message.Content,
            SentAt: DateTime.UtcNow);

        await Clients.Group(message.ChatId).ReceiveMessage(new ClientMessage(username, message.Content));
        
        await eventService.PublishAsync(messageSentEvent);
    }
}