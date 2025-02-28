using Saas.Application.Contracts;
using Saas.Domain;

namespace Saas.Api.Contracts;

public sealed record GroupDto(
    Guid Id,
    string Name,
    List<UserInformationDto> Members, 
    UserInformationDto? Creator)
{
    internal static GroupDto From(Group group) =>
        new(Id: group.Id,
            Name: group.Name.Value,
            Members: group.Members.Select(UserInformationDto.From).ToList(),
            Creator: group.Creator is null ? null : UserInformationDto.From(group.Creator));
}