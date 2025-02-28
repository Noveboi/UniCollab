﻿using Ardalis.Result;
using Saas.Application.Interfaces;
using Saas.Application.Interfaces.Data;
using Saas.Domain;

namespace Saas.Application.UseCases.Users;

public class GetUserUseCase(IUserRepository repository) : IApplicationUseCase
{
    public async Task<Result<User>> Handle(Guid userId)
    {
        var user = await repository.GetByIdAsync(userId);
        if (user is null) 
            return Result<User>.NotFound();

        return user;
    }
}