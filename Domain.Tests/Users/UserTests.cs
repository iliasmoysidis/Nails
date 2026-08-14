using Domain.Common;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjects;
using Domain.Users;
using Moq;

namespace Domain.Tests.Users;

public class UserTests
{
    private readonly Mock<IClock> _clock;

    public UserTests()
    {
        _clock = new Mock<IClock>();
    }

    [Fact]
    public void Constructor_ShouldCreateActiveUser()
    {
        // Arrange
        var fullName = FullName.From("Ilias", "Moysidis");
        var email = Email.From("iliamous92@gmail.com");
        var phone = Phone.From("+30", "6972276320");

        // Act
        var user = new User(fullName, email, phone);

        // Assert
        Assert.Equal(fullName, user.FullName);
        Assert.Equal(email, user.Email);
        Assert.Equal(phone, user.Phone);
        Assert.False(user.IsDeleted);
        Assert.Null(user.DeletedAt);
    }

    [Fact]
    public void Delete_ShouldMarkUserAsDeleted()
    {
        // Arrange
        var fullName = FullName.From("Ilias", "Moysidis");
        var email = Email.From("iliamous92@gmail.com");
        var phone = Phone.From("+30", "6972276320");
        var user = new User(fullName, email, phone);

        var expectedTime = UtcDateTime.FromUtc(
            new DateTime(2026, 8, 14, 12, 0, 0, 0, DateTimeKind.Utc)
        );
        _clock
            .Setup(x => x.Now)
            .Returns(expectedTime);

        // Act
        user.Delete(_clock.Object);

        // Assert
        Assert.True(user.IsDeleted);
        Assert.Equal(expectedTime, user.DeletedAt);
    }

    [Fact]
    public void Delete_ShouldThrowWhenUserIsAlreadyDeleted()
    {
        var fullName = FullName.From("Ilias", "Moysidis");
        var email = Email.From("iliamous92@gmail.com");
        var phone = Phone.From("+30", "6972276320");
        var user = new User(fullName, email, phone);

        var expectedTime = UtcDateTime.FromUtc(
            new DateTime(2026, 8, 14, 12, 0, 0, 0, DateTimeKind.Utc)
        );
        _clock
            .Setup(x => x.Now)
            .Returns(expectedTime);
        user.Delete(_clock.Object);

        // Act

        var exception = Record.Exception(
            () => user.Delete(_clock.Object)
        );

        // Assert
        Assert.IsType<InvariantException>(exception);
        Assert.Equal("User is deleted.", exception.Message);
    }
}
