using System.Text.Json;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saas.Api.Contracts;
using Saas.Api.Contracts.Requests;
using Saas.Application.UseCases.ChatRooms;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Saas.Api.Endpoints;

[ApiController]
[Route("/chat")]
[Authorize]
public class ChatRoomController : ControllerBase
{
    /// <summary>
    /// Retrieve all the chat rooms that the user can join.
    /// </summary>
    /// <returns>A list of chatrooms</returns>
    [HttpGet("joinable/{userId:guid}")]
    public async Task<IResult> GetJoinable(Guid userId, [FromServices] GetJoinableChatRooms getJoinableChatRooms)
    {
        var result = await getJoinableChatRooms.Handle(userId);
        if (!result.IsSuccess)
            return result.ToMinimalApiResult();

        var chatRooms = result.Value;
        return Results.Ok(chatRooms.Select(ChatRoomInformationDto.From));
    }
    
    /// <summary>
    /// Retrieve a chatroom by their ID, if found.
    /// </summary>
    /// <param name="chatRoomId">The ID to search.</param>
    /// <param name="getChatRoom"></param>
    /// <returns>A chat room</returns>
    [HttpGet("{chatRoomId:guid}")]
    public async Task<IResult> Get(
        [FromRoute] Guid chatRoomId, 
        [FromServices] GetChatRoomUseCase getChatRoom)
    {
        var result = await getChatRoom.Handle(chatRoomId);
        if (!result.IsSuccess)
            return result.ToMinimalApiResult();
    
        var chatRoom = result.Value;
        return Results.Ok(ChatRoomDto.From(chatRoom));
    }

    [HttpPost]
    public async Task<IResult> Create(
        [FromBody] CreateChatRoomRequest request, 
        [FromServices] CreateChatRoomUseCase createChatRoom)
    {
        var result = await createChatRoom.Handle(
            name: request.Name,
            userIds: request.InitialParticipants.Select(p => p.Id).ToList());

        if (!result.IsSuccess)
            return result.ToMinimalApiResult();

        var chatRoom = result.Value;
        return Results.CreatedAtRoute(
            routeName: "/chat",
            routeValues: chatRoom.Id,
            value: chatRoom);
    }
}