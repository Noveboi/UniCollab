﻿using Ardalis.Result;
using FluentAssertions;
using Saas.Tests.Fakes;

namespace Saas.Domain.Tests;

public class UserTests
{
    [Fact]
    public void AddFriend_Should_AddUserToFriendList()
    {
        // Arrange
        var user1 = FakeUsers.Generate();
        var user2 = FakeUsers.Generate();

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
        var user2 = FakeUsers.Generate();
        var user1 = FakeUsers.Generate(friends: [user2]);

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
        var user1 = FakeUsers.Generate();
        var user2 = FakeUsers.Generate();

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
        var user = FakeUsers.Generate();

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
        var user2 = FakeUsers.Generate();
        var user1 = FakeUsers.Generate(friends: [user2]);

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
        var user1 = FakeUsers.Generate();
        var user2 = FakeUsers.Generate();
        
        var group = new Group("Test", [user1], user1);
        
        // Act
        var result = user2.JoinGroup(group);
        
        // Assert
        group.Members.Should().Contain(user2);
        
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void LeaveGroup_Should_SuccessfullyRemoveUserFromGroup()
    {
        // Arrange
        var user1 = FakeUsers.Generate();
        var user2 = FakeUsers.Generate();
        
        var group = new Group("Test", [user1, user2], user1);
        
        // Act
        var result = user2.LeaveGroup(group);
        
        // Assert
        group.Members.Should().NotContain(user2);
        
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void JoinChat_Should_SuccessfullyAddUserToChat()
    {
        // Arrange
        var user1 = FakeUsers.Generate();
        var user2 = FakeUsers.Generate();
        
        var chatroom = new ChatRoom("test", "test", [user1], []);
        
        // Act
        var result = user2.JoinChat(chatroom);
        
        // Assert
        chatroom.Participants.Should().Contain(user2);
        
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void SendMessage_Should_SuccessfullyAddMessageToChat()
    {
        // Arrange
        var user = FakeUsers.Generate();
        var message = new Message("Test", DateTime.Now, user);
        var chatroom = new ChatRoom("test", "test", [user], []);
        
        // Act
        var result = user.SendMessage(chatroom, message);
        
        // Assert
        chatroom.Messages.Should().Contain(message);
        
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void DeleteMessage_Should_SuccessfullyRemoveMessageFromChat()
    {
        // Arrange 
        var user = FakeUsers.Generate();
        var message = new Message("Test", DateTime.Now, user);
        var chatroom = new ChatRoom("test", "test", [user], [message]);
        
        // Act
        var result = user.DeleteMessage(chatroom, message);
        
        // Assert
        chatroom.Messages.Should().NotContain(message);
        
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public void LeaveChat_Should_SuccessfullyRemoveUserFromChat()
    {
        // Arrange
        var user = FakeUsers.Generate();
        var chatroom = new ChatRoom("test", "test", [user], []);
        
        // Act
        var result = user.LeaveChat(chatroom);
        
        // Assert
        chatroom.Participants.Should().NotContain(user);
        
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void CreatePost_Should_SuccessfullyCreatePost()
    {
        // Arrange
        var user = FakeUsers.Generate();
        
        // Act
        var result = user.CreatePost("Test", "Test", []);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void CreateGroup_Should_SuccessfullyCreateGroup()
    {
        // Arrange
        var user = FakeUsers.Generate();
        
        // Act
        var result = user.CreateGroup("test");
        
        // Assert
        // Idk what to do!
    }
}