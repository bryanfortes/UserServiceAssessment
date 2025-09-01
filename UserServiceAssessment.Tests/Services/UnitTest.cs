using FluentAssertions;
using Moq;
using UserServiceAssessment.Data.Interfaces;
using UserServiceAssessment.Helpers.Exceptions;
using UserServiceAssessment.Models.User;
using UserServiceAssessment.Services;


namespace UserServiceAssessment.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserContext> _userContextMock;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userContextMock = new Mock<IUserContext>();
        _userService = new UserService(_userContextMock.Object);
    }
    #region Positive Tests
    [Fact]
    public void AddUser_ValidUser_ReturnsUserId()
    {
        // Arrange
        var createUser = new CreateUserModel { Name = "Test User", Email = "test@example.com" };
        _userContextMock.Setup(x => x.Add(createUser)).Returns(1);

        // Act
        var result = _userService.AddUser(createUser);

        // Assert
        result.Should().Be(1);
        _userContextMock.Verify(x => x.Add(createUser), Times.Once);
    }

    [Fact]
    public void RemoveUserById_ExistingUser_ReturnsTrue()
    {
        // Arrange
        _userContextMock.Setup(x => x.Remove(1)).Returns(true);

        // Act
        var result = _userService.RemoveUserById(1);

        // Assert
        result.Should().BeTrue();
        _userContextMock.Verify(x => x.Remove(1), Times.Once);
    }

    [Fact]
    public void GetUserById_ExistingUser_ReturnsUser()
    {
        // Arrange
        var expectedUser = new UserModel { Id = 1, Name = "Test User", Email = "test@example.com" };
        _userContextMock.Setup(x => x.Get(1)).Returns(expectedUser);

        // Act
        var result = _userService.GetUserById(1);

        // Assert
        result.Should().BeEquivalentTo(expectedUser);
    }
    [Fact]
    public void UpdateUserEmailById_ExistingUser_ReturnsUpdatedUser()
    {
        // Arrange
        var userId = 1;
        var newEmail = "updated@example.com";
        var updatedUser = new UserModel { Id = userId, Name = "Test User", Email = newEmail };

        _userContextMock.Setup(x => x.UpdateUserEmailById(userId, newEmail)).Returns(true);
        _userContextMock.Setup(x => x.Get(userId)).Returns(updatedUser);

        // Act
        var result = _userService.UpdateUserEmailById(userId, newEmail);

        // Assert
        result.Should().BeEquivalentTo(updatedUser);
        _userContextMock.Verify(x => x.UpdateUserEmailById(userId, newEmail), Times.Once);
        _userContextMock.Verify(x => x.Get(userId), Times.Once);
    }
    #endregion

    #region Negative Tests
    [Fact]
    public void AddUser_EmptyEmail_ThrowsArgumentException()
    {
        var invalidUser = new CreateUserModel { Name = "Test User", Email = "" };
        _userContextMock.Setup(x => x.Add(invalidUser)).Throws(new ArgumentException("Invalid email format. \"\" is not a valid email."));

        // Act
        Action act = () => _userService.AddUser(invalidUser);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format. \"\" is not a valid email.");
    }

    [Fact]
    public void AddUser_InvalidEmail_ThrowsArgumentException()
    {
        var invalidUser = new CreateUserModel { Name = "Test User", Email = "invalid.email" };
        _userContextMock.Setup(x => x.Add(invalidUser)).Throws(new ArgumentException("Invalid email format. \"invalid.email\" is not a valid email."));

        // Act
        Action act = () => _userService.AddUser(invalidUser);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format. \"invalid.email\" is not a valid email.");
    }

    [Fact]
    public void AddUser_EmptyName_ThrowsArgumentException()
    {
        var invalidUser = new CreateUserModel { Name = "", Email = "test@example.com" };
        _userContextMock.Setup(x => x.Add(invalidUser));

        // Act
        Action act = () => _userService.AddUser(invalidUser);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name cannot be blank.");
    }

    [Fact]
    public void AddUser_InvalidName_ThrowsArgumentException()
    {
        var invalidUser = new CreateUserModel { Name = "Name123", Email = "test@example.com" };
        _userContextMock.Setup(x => x.Add(invalidUser));

        // Act
        Action act = () => _userService.AddUser(invalidUser);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Name can only contain letters, spaces, and hyphens.");
    }

    [Fact]
    public void GetUserById_NonExistingUser_ThrowsUserNotFoundException()
    {
        // Arrange
        _userContextMock.Setup(x => x.Get(1)).Returns((UserModel?)null);

        // Act & Assert
        var action = () => _userService.GetUserById(1);
        action.Should().Throw<UserNotFoundException>()
            .WithMessage("User with the Id of 1 was not found.");
    }

    [Fact]
    public void RemoveUserById_NonExistingUser_ThrowsUserNotFoundException()
    {
        // Arrange
        _userContextMock.Setup(x => x.Remove(99)).Returns(false);

        // Act
        Action act = () => _userService.RemoveUserById(99);

        // Assert
        act.Should().Throw<UserNotFoundException>()
            .WithMessage("User with the Id of 99 was not found.");
    }
    [Fact]
    public void UpdateUserEmailById_NonExistingUser_ThrowsUserNotFoundException()
    {
        // Arrange
        var userId = 99;
        var newEmail = "new@example.com";
        _userContextMock.Setup(x => x.UpdateUserEmailById(userId, newEmail)).Returns(false);

        // Act
        Action act = () => _userService.UpdateUserEmailById(userId, newEmail);

        // Assert
        act.Should().Throw<UserNotFoundException>()
            .WithMessage("User with the Id of 99 was not found.");
    }
    [Fact]
    public void UpdateUserEmailById_InvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        var userId = 1;
        var invalidEmail = "invalid.email";

        // Act
        Action act = () => _userService.UpdateUserEmailById(userId, invalidEmail);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format. \"invalid.email\" is not a valid email.");
    }

    [Fact]
    public void UpdateUserEmailById_EmptyEmail_ThrowsArgumentException()
    {
        // Arrange
        var userId = 1;
        var emptyEmail = "";

        // Act
        Action act = () => _userService.UpdateUserEmailById(userId, emptyEmail);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format. \"\" is not a valid email.");
    }
    #endregion
}