using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.Exceptions;
using JobMagnet.Application.UseCases.Auth;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Domain.Aggregates;
using Microsoft.Extensions.Options;
using Moq;

namespace JobMagnet.Unit.Tests.UseCases;

public class AuthUserHandlerShould
{
    private readonly IFixture _fixture;
    private readonly Mock<IUserManagerAdapter> _userManagerAdapterMock;
    private readonly AuthUserHandler _authUserHandler;
    private readonly Mock<IOptions<AdminUserOptions>> _optionsMock;

    public AuthUserHandlerShould()
    {
        _fixture = new Fixture();
        _optionsMock = new Mock<IOptions<AdminUserOptions>>();
        _userManagerAdapterMock = new Mock<IUserManagerAdapter>();
        _authUserHandler =  new AuthUserHandler(_userManagerAdapterMock.Object, _optionsMock.Object);
    }
    
    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsUserToken()
    {
        // --- Given ---
        var loginDto = _fixture.Create<UserModelCredentials>();
        var expectedToken = new UserToken
        {
            Token = "un_jwt_token_valido",
            Expiration = DateTime.UtcNow.AddHours(1)
        };

        _userManagerAdapterMock.Setup(x => x.LoginAsync(loginDto))
            .ReturnsAsync(expectedToken);
        
        // --- When  ---
        var result = await _authUserHandler.LoginAsync(loginDto);

        // --- Then  ---
        result.Should().NotBeNull();
        result.Token.Should().BeEquivalentTo(expectedToken.Token);
        
        _userManagerAdapterMock.Verify(
            adapter => adapter.LoginAsync(loginDto), 
            Times.Once);
    }
    
    [Fact]
    public async Task LoginAsync_WhenAdapterReturnsNull_ReturnsNull()
    {
        // --- Given ---
        var loginDto = _fixture.Create<UserModelCredentials>();
        _userManagerAdapterMock.Setup(x => x.LoginAsync(It.IsAny<UserModelCredentials>())).ReturnsAsync((UserToken)null);

        // --- When ---
        var result = await _authUserHandler.LoginAsync(loginDto);

        // --- Then ---
        result.Should().BeNull();
    }
    
    [Theory]
    [InlineData(null, "password")] 
    [InlineData("test@email.com", null)]
    [InlineData("", "password")]
    [InlineData("test@email.com", "")]
    [InlineData("   ", "password")]
    [InlineData("test@email.com", "   ")] 
    [InlineData("", "")]
    public async Task LoginAsync_WhenEmailOrPasswordIsEmpty_ThrowsArgumentException(string email, string password)
    {
        // --- Given ---
        var loginDto = new UserModelCredentials { Email = email, Password = password };
        
        // --- When ---
        Func<Task> action = () => _authUserHandler.LoginAsync(loginDto);
        
        // --- Then ---
        await action.Should().ThrowAsync<ArgumentException>()
            .WithMessage("The email and password cannot be null, empty, or contain only spaces.");
    }
    
    [Fact]
    public async Task CreateAdminUserAsync_WhenCalled_ReturnsUserToken()
    {
        // --- Given ---
        var expectedToken = new UserToken
        {
            Token = "token_admin",
            Expiration = DateTime.UtcNow.AddHours(1)
        };
        _userManagerAdapterMock.Setup(x => x.CreateAdminUserAsync(It.IsAny<AdminUserOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedToken);
        
        // --- When ---
        var result = await _authUserHandler.CreateAdminUserAsync(CancellationToken.None);

        // --- Then ---
        result.Should().NotBeNull();
        result.Token.Should().BeEquivalentTo(expectedToken.Token);
        result.Expiration.Should().BeCloseTo(expectedToken.Expiration, TimeSpan.FromSeconds(1));
    }
    
    
    [Fact]
    public async Task CreateAdminUserAsync_WhenEmailAlreadyTaken_ThrowsAdminUserAlreadyExistsException()
    {
        // --- Given ---
        var adminUserOptions = new AdminUserOptions
        {
            UserName = "admin",
            Email = "admin@demo.com",
            Password = "Admin123!"
        };
        _optionsMock.Setup(o => o.Value).Returns(adminUserOptions);
    
        var innerException = new Exception("Email is already taken");
        _userManagerAdapterMock
            .Setup(x => x.CreateAdminUserAsync(adminUserOptions, It.IsAny<CancellationToken>()))
            .ThrowsAsync(innerException);
    
        // --- When ---
        Func<Task> action = () => _authUserHandler.CreateAdminUserAsync(CancellationToken.None);
    
        // --- Then ---
        await action.Should().ThrowAsync<AdminUserAlreadyExistsException>()
            .WithMessage($"The administrator user with the email '{adminUserOptions.Email}' already exists.");
    }
    
    // Test Register
    
    [Fact]
    public async Task RegisterAsync_ShouldThrowArgumentNullException_WhenCredentialsAreNull()
    {
        // --- When ---
        Func<Task> act = async () => await _authUserHandler.RegisterAsync(null);
        
        // --- Then ---
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task RegisterAsync_ShouldThrowArgumentException_WhenEmailIsInvalid(string invalidEmail)
    {
        // --- Given ---
        var credentials = new UserModelCredentials 
        { 
            Email = invalidEmail, 
            Password = "SomeValidPassword123" 
        };
        // --- When ---
        Func<Task> act = async () => await _authUserHandler.RegisterAsync(credentials);
        
        // --- Then ---
        await act.Should().ThrowAsync<ArgumentException>();
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldCallAdapterAndReturnToken_WhenCredentialsAreValid()
    {
        // --- Given ---
        var validCredentials = new UserModelCredentials
        {
            Email = "test@example.com",
            Password = "ValidPassword123!"
        };
        var expectedToken = new UserToken 
        { 
            Token = "fake-jwt-token", 
            Expiration = DateTime.UtcNow.AddHours(1) 
        };
        
        _userManagerAdapterMock
            .Setup(adapter => adapter.RegisterAsync(validCredentials))
            .ReturnsAsync(expectedToken);
        
        // --- When ---
        var result = await _authUserHandler.RegisterAsync(validCredentials);

        // --- Then ---
        result.Should().NotBeNull();
        result.Should().Be(expectedToken);
        result.Token.Should().Be("fake-jwt-token");
        
        _userManagerAdapterMock.Verify(
            adapter => adapter.RegisterAsync(validCredentials), 
            Times.Once);
    }
}