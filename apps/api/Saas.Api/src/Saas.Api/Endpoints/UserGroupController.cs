using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saas.Api.Contracts;
using Saas.Api.Contracts.Queries;
using Saas.Api.Contracts.Requests;
using Saas.Api.Extensions;
using Saas.Application.UseCases.Groups;

namespace Saas.Api.Endpoints;

/// <summary>
/// How a user can interact with the group.
/// </summary>
[ApiController]
[Route("/users/{userId:guid}/groups")]
[Authorize]
public class UserGroupController : ControllerBase
{
    /// <summary>
    /// Gets the groups of a user. You can query parameters to specify what types of groups you want returned.
    /// </summary>
    [HttpGet]
    public async Task<IResult> Get(
        [FromRoute] Guid userId,
        [FromServices] GetGroupQuery query,
        [FromServices] GetGroups getGroups)
    {
        return query.QueryType switch
        {
            GroupQueryType.Joinable => (await getGroups.Joinable(userId)).ToHttp(
                onSuccess: groups => groups.Select(GroupInformationDto.From)),
            GroupQueryType.Participating => (await getGroups.Participating(userId)).ToHttp(
                onSuccess: groups => groups.Select(GroupInformationDto.From)),
            _ => Results.BadRequest("Unknown 'Type' parameter.")
        };
    }
    
    /// <summary>
    /// Attempts to insert the specified user in the specified group.
    /// </summary>
    [HttpPost("{groupId:guid}", Name = "Join Group")]
    public async Task<IResult> JoinGroup(
        [FromRoute] Guid userId, 
        [FromRoute] Guid groupId,
        [FromServices] JoinGroup joinGroup)
    {
        var result = await joinGroup.HandleAsync(groupId, userId);
        return !result.IsSuccess 
            ? result.ToMinimalApiResult() 
            : Results.Ok();
    }
    
    /// <summary>
    /// Attempts to remove the specified user from the specified group.
    /// </summary>
    [HttpDelete("{groupId:guid}", Name = "Leave Group")]
    public async Task<IResult> LeaveGroup(
        [FromRoute] Guid userId, 
        [FromRoute] Guid groupId,
        [FromServices] LeaveGroup leaveGroup)
    {
        var result = await leaveGroup.HandleAsync(groupId, userId);
        return !result.IsSuccess 
            ? result.ToMinimalApiResult() 
            : Results.Ok();
    }
}