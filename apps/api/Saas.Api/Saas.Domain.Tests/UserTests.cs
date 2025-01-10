﻿using Ardalis.Result;
using FluentAssertions;

namespace Saas.Domain.Tests;

public class UserTests
{
    [Fact]
    public void AddFriend_Should_AddUserToFriendList()
    {
        // Arrange
        var user1 = new User(Guid.NewGuid(), "Test", "password", []);
        var user2 = new User(Guid.NewGuid(), "Test 2", "password", []);

        var initialFriendCount = user1.Friends.Count;

        // Act
        var result = user1.AddFriend(user2);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        user1.Friends.Should().Contain(user2);
        user1.Friends.Count.Should().Be(initialFriendCount + 1);
    }

    [Fact]
    public void RemoveFriend_Should_RemoveUserFromFriendList()
    {
        // Arrange
        var user2 = new User(Guid.NewGuid(), "Test 2", "password", []);
        var user1 = new User(Guid.NewGuid(), "Test", "password", [user2]);

        // Act
        var result = user1.RemoveFriend(user2);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user1.Friends.Should().BeEmpty();
    }

    [Fact]
    public void RemoveFriend_ShouldReturnNotFound_WhenUserIsNotInFriendList()
    {
        // Arrange
        var user1 = new User(Guid.NewGuid(), "Test", "password", []);
        var user2 = new User(Guid.NewGuid(), "Test 2", "password", []);

        // Act
        var result = user1.RemoveFriend(user2);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsNotFound().Should().BeTrue();
    }

    [Fact]
    public void AddFriend_ShouldReturnInvalid_WhenUserAddsThemselves()
    {
        // Arrange
        var user = new User(Guid.NewGuid(), "Test", "", []);

        // Act
        var result = user.AddFriend(user);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsInvalid().Should().BeTrue();
    }

    [Fact]
    public void AddFriend_ShouldReturnConflict_WhenUserIsAlreadyAFriend()
    {
        // Arrange
        var user2 = new User(Guid.NewGuid(), "Test 2", "password", []);
        var user1 = new User(Guid.NewGuid(), "Test", "password", [user2]);

        // Act
        var result = user1.AddFriend(user2);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsConflict().Should().BeTrue();
    }

    [Fact]
    public void JoinGroup_Should_SuccessfullyAddUserToGroup()
    {
        // Arrange
        var user1 = new User(Guid.NewGuid(), "Test", "password", []);
        var user2 = new User(Guid.NewGuid(), "Test 2", "password", []);
        var group = new Group(Guid.NewGuid(), "Test", [user1], user1);
        
        // Act
        var result = user2.JoinGroup(group);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void LeaveGroup_Should_SuccessfullyRemoveUserFromGroup()
    {
        // Arrange
        var user1 = new User(Guid.NewGuid(), "Test", "password", []);
        var user2 = new User(Guid.NewGuid(), "Test 2", "password", []);
        var group = new Group(Guid.NewGuid(), "Test", [user1, user2], user1);
        
        // Act
        var result = user2.LeaveGroup(group);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}