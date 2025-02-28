using Ardalis.Result;
using FluentAssertions;
using Saas.Tests.Fakes;

namespace Saas.Domain.Tests.ChatRooms;

public class ChatRoomBehaviors
{
    [Fact]
    public void AddParticipant_Should_AddUserToTheParticipantsList()
    {
        // Arrange
        var user1 = FakeUsers.Generate();
        var user2 = FakeUsers.Generate();

        var chatRoom = FakeChatRooms.Generate(participants: [user2]); 
        var initialparticipantsCount = chatRoom.Participants.Count;
        
        // Act
        var result = chatRoom.AddParticipant(user1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        chatRoom.Participants.Should().Contain(user1);
        chatRoom.Participants.Count.Should().Be(initialparticipantsCount + 1);
    }

    [Fact]
    public void AddParticipant_Should_ReturnConflict_WhenUserIsAlreadyAParticipant()
    {
        // Arrange
        var user1 = FakeUsers.Generate();
        var user2 = FakeUsers.Generate();

        var chatRoom = FakeChatRooms.Generate(participants: [user1, user2]);
        
        // Act
        var result = chatRoom.AddParticipant(user1);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsConflict().Should().BeTrue();
    }

    [Fact]
    public void RemoveParticipant_Should_RemoveParticipantFromTheParticipantsList()
    {
        // Arrange
        var user = FakeUsers.Generate();
        var chatRoom = FakeChatRooms.Generate(participants: [user]);
        
        var initialparticipantsCount = chatRoom.Participants.Count;
        
        // Act
        var result = chatRoom.RemoveParticipant(user);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        chatRoom.Participants.Should().NotContain(user);
        chatRoom.Participants.Count.Should().Be(initialparticipantsCount - 1);
    }

    [Fact]
    public void RemoveParticipant_Should_ReturnNotFound_WhenUserIsNotInParticipantsList()
    {
        // Arrange
        var user1 = FakeUsers.Generate();
        var user2 = FakeUsers.Generate(friends: [user1]);

        var chatRoom = FakeChatRooms.Generate(participants: [user2]);
        
        // Act
        var result = chatRoom.RemoveParticipant(user1);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsNotFound().Should().BeTrue();
    }

    [Fact]
    public void AddMessage_Should_AddMessageToTheMessagesList()
    {
        // Arrange
        var user = FakeUsers.Generate();
        var message = new Message("Test", DateTime.Now, user);
        var chatRoom = FakeChatRooms.Generate(participants: [user]);
        
        var initialMessagesCount = chatRoom.Messages.Count;
        
        // Act
        var result = chatRoom.AddMessage(message);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        chatRoom.Messages.Should().Contain(message);
        chatRoom.Messages.Count.Should().Be(initialMessagesCount + 1);
    }

    [Fact]
    public void AddMessage_Should_ReturnConflict_WhenMessageIsAlreadyInMessagesList()
    {
        // Arrange
        var user = FakeUsers.Generate();

        var message = new Message("Test", DateTime.Now, user);
        
        var chatRoom = FakeChatRooms.Generate(participants: [user], messages: [message]);
        
        // Act
        var result = chatRoom.AddMessage(message);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsConflict().Should().BeTrue();
    }

    [Fact]
    public void DeleteMessage_Should_RemoveMessageFromTheMessagesList()
    {
        // Arrange
        var user = FakeUsers.Generate();
        var message = new Message("Test", DateTime.Now, user);
        var chatRoom = FakeChatRooms.Generate(participants: [user], messages: [message]);
        
        var initialMessagesCount = chatRoom.Messages.Count;
        
        // Act
        var result = chatRoom.DeleteMessage(message);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        chatRoom.Messages.Should().NotContain(message);
        chatRoom.Messages.Count.Should().Be(initialMessagesCount - 1);
    }

    [Fact]
    public void DeleteMessage_Should_ReturnNotFound_WhenMessageIsNotInTheMessagesList()
    {
        // Arrange
        var user = FakeUsers.Generate();
        var message = new Message("Test", DateTime.Now, user);
        var chatRoom = FakeChatRooms.Generate(participants: [user], messages: []);
        
        // Act
        var result = chatRoom.DeleteMessage(message);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsNotFound().Should().BeTrue();
    }
}