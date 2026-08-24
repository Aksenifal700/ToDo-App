

using Moq;
using TodoApp.BusinessLogic.Exceptions;
using TodoApp.BusinessLogic.Security;
using TodoApp.BusinessLogic.Services;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Auth;

namespace TodoApp.UnitTests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _authService = new AuthService(
            _userRepositoryMock.Object,
            _jwtTokenGeneratorMock.Object,
            _passwordHasherMock.Object);
    }

    [Fact]
    public async Task LoginAsync_WhenUserDoesNotExist_ThrowsInvalidCredentialException()
    {
        // Arrange
        var dto = new LoginDto { Email = "test@test.com", Password = "password123" };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((UserDto?)null);

        // Act  
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            () => _authService.LoginAsync(dto));

        //Assert
        _passwordHasherMock.Verify(
            x => x.VerifyPasswordHash(It.IsAny<string>(), 
                It.IsAny<byte[]>(), 
                It.IsAny<byte[]>()),
            Times.Never);
        _jwtTokenGeneratorMock.Verify(
            x => x.GenerateJwtToken(It.IsAny<TokenGenerationDto>()),
            Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsInvalid_ThrowsInvalidCredentialException()
    {
        // Arrange
        var dto = new LoginDto { Email = "test@test.com", Password = "wrongPassword" };
        var user = new UserDto
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 }
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(x => x.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            .Returns(false);

        // Act
        var exception = await Record.ExceptionAsync(() => _authService.LoginAsync(dto));

        // Assert
        Assert.IsType<InvalidCredentialsException>(exception);
        _jwtTokenGeneratorMock.Verify(
            x => x.GenerateJwtToken(It.IsAny<TokenGenerationDto>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsLoginResultWithToken()
    {
        // Arrange
        var dto = new LoginDto { Email = "test@test.com", Password = "correctPassword" };
        var userId = Guid.NewGuid();
        var user = new UserDto
        {
            Id = userId,
            Email = dto.Email,
            PasswordHash = new byte[] { 1, 2, 3 },
            PasswordSalt = new byte[] { 4, 5, 6 }
        };
        const string expectedToken = "fake-jwt-token";

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _passwordHasherMock
            .Setup(x => x.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            .Returns(true);

        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateJwtToken(It.Is<TokenGenerationDto>(t => 
                t.UserId == userId && t.Email == dto.Email)))
            .Returns(expectedToken);

        // Act
        var result = await _authService.LoginAsync(dto);

        // Assert
        Assert.Equal(userId, result.UserId);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(expectedToken, result.Token);
    }
    
    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsEmailAlreadyExistsException()
    {
        // Arrange
        var dto = new RegisterDto { Email = "test@test.com", Password = "password123" };

        _userRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(dto.Email))
            .ReturnsAsync(true);

        // Act
        var exception = await Record.ExceptionAsync(() => _authService.RegisterAsync(dto));

        // Assert
        Assert.IsType<AlreadyExistsException>(exception);
        _userRepositoryMock.Verify(
            x => x.CreateUserAsync(It.IsAny<RegisterDto>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()),
            Times.Never);
        _jwtTokenGeneratorMock.Verify(
            x => x.GenerateJwtToken(It.IsAny<TokenGenerationDto>()), Times.Never);
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailIsUnique_CreatesUserAndReturnsLoginResult()
    {
        // Arrange
        var dto = new RegisterDto { Email = "new@test.com", Password = "password123" };
        var userId = Guid.NewGuid();
        var createdUser = new UserDto { Id = userId, Email = dto.Email };
        const string expectedToken = "fake-jwt-token";

        _userRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(dto.Email))
            .ReturnsAsync(false);

        _passwordHasherMock
            .Setup(x => x.CreatePasswordHash(
                dto.Password,
                out It.Ref<byte[]>.IsAny,
                out It.Ref<byte[]>.IsAny));

        _userRepositoryMock
            .Setup(x => x.CreateUserAsync(dto, It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .ReturnsAsync(createdUser);

        _jwtTokenGeneratorMock
            .Setup(x => x.GenerateJwtToken(It.Is<TokenGenerationDto>(t =>
                t.UserId == userId && t.Email == dto.Email)))
            .Returns(expectedToken);

        // Act
        var result = await _authService.RegisterAsync(dto);

        // Assert
        Assert.Equal(userId, result.UserId);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(expectedToken, result.Token);
    }
}