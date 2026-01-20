using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.UseCases.Auth;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Domain.Aggregates;
using Microsoft.Extensions.Options;
using JobMagnet.Application.Exceptions;
using Moq;

namespace JobMagnet.Unit.Tests.UseCases;

public class AuthUserHandlerShould
{
    private readonly IFixture _fixture;
    private readonly AuthUserHandler _authUserHandler;
    private readonly Mock<IUserManage> _userManagerMock;
    private readonly Mock<IOptions<AdminUserOptions>> _optionsMock;
    private readonly CancellationToken _cancellationToken;

    public AuthUserHandlerShould()
    {
        _fixture = new Fixture();
        _userManagerMock = new Mock<IUserManage>(); 
        _optionsMock = new Mock<IOptions<AdminUserOptions>>();
        _authUserHandler =  new AuthUserHandler(_userManagerMock.Object, _optionsMock.Object);
        _cancellationToken = CancellationToken.None;
    }
    
    // Test Register //
    
    [Fact]
    public async Task RegisterAsync_ShouldReturnToken_WhenCredentialsAreValidAndEmailDoesNotExist()
    {
        // --- Given ---
        var validCredentials = new UserModelCredentialsDto
        {
            Email = "test@example.com",
            Password = "ValidPassword123!"
        };
        var expectedToken = new UserTokenDto 
        { 
            Token = "fake-jwt-token", 
            Expiration = DateTime.UtcNow.AddHours(1) 
        };
        
        _userManagerMock
            .Setup(um => um.EmailExistAsync(validCredentials.Email))
            .ReturnsAsync(false);
        
        _userManagerMock
            .Setup(um => um.RegisterAsync(validCredentials, _cancellationToken))
            .ReturnsAsync(expectedToken);
        
        // --- When ---
        var result = await _authUserHandler.RegisterAsync(validCredentials,  _cancellationToken);
    
        // --- Then ---
        result.Should().NotBeNull();
        result.Should().Be(expectedToken); 
        
        _userManagerMock.Verify(um => um.EmailExistAsync(validCredentials.Email), Times.Once);
        _userManagerMock.Verify(um => um.RegisterAsync(validCredentials, _cancellationToken),Times.Once);
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldThrowJobMagnetApplicationException_WhenEmailAlreadyExists()
    {
        // --- Given ---
        var existingCredentials = new UserModelCredentialsDto
        {
            Email = "existinguser@example.com",
            Password = "AnotherPassword123!"
        };

        _userManagerMock
            .Setup(um => um.EmailExistAsync(existingCredentials.Email))
            .ReturnsAsync(true);

        // --- Act & Then ---
        
        Func<Task> act = async () => await _authUserHandler.RegisterAsync(existingCredentials, _cancellationToken);
        
        await act.Should().ThrowAsync<JobMagnetApplicationException>()
            .WithMessage("Email already exists.");
        
        _userManagerMock.Verify(um => um.RegisterAsync(It.IsAny<UserModelCredentialsDto>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [InlineData("", "password")]
    [InlineData("test@test.com", "")]
    [InlineData(" ", "password")]
    [InlineData("test@test.com", " ")]
    public async Task RegisterAsync_ShouldThrowArgumentException_WhenEmailOrPasswordIsWhitespace(string email, string password)
    {
        // --- Given ---
        var invalidCredentials  = new UserModelCredentialsDto 
        { 
            Email = email, Password = password
        };
        // --- When & Then ---
        Func<Task> act = async () => await _authUserHandler.RegisterAsync(invalidCredentials, _cancellationToken);
        
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Password and email are required.");
        
        _userManagerMock.VerifyNoOtherCalls();
    }
    
    /// Test Login ///
    
    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsUserToken()
    {
        // --- Given ---
        var loginDto = _fixture.Create<UserModelCredentialsDto>();
        var expectedToken = new UserTokenDto
        {
            Token = "un_jwt_token_valido",
            Expiration = DateTime.UtcNow.AddHours(1)
        };
    
        _userManagerMock.Setup(x => x.LoginAsync(loginDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedToken);
        
        // --- When  ---
        var result = await _authUserHandler.LoginAsync(loginDto, _cancellationToken);
    
        // --- Then  ---
        result.Should().NotBeNull();
        result.Should().Be(expectedToken);
        result.Token.Should().Be(expectedToken.Token);
        
        _userManagerMock.Verify(
            adapter => adapter.LoginAsync(loginDto, It.IsAny<CancellationToken>()), 
            Times.Once);
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
        var loginDto = new UserModelCredentialsDto { Email = email, Password = password };
        
        // --- When ---
        Func<Task> action = () => _authUserHandler.LoginAsync(loginDto, _cancellationToken);
        
        // --- Then ---
        await action.Should().ThrowAsync<ArgumentException>()
            .WithMessage("The email and password cannot be null, empty, or contain only spaces.");
        
        _userManagerMock.Verify(
            adapter => adapter.LoginAsync(It.IsAny<UserModelCredentialsDto>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }
    
    // Test AdminUser //
    
    [Fact]
    public async Task CreateAdminUserAsync_WhenCalled_ReturnsUserToken()
    {
        // --- Given ---
        var expectedToken = new UserTokenDto
        {
            Token = "token_admin",
            Expiration = DateTime.UtcNow.AddHours(1)
        };
        _userManagerMock.Setup(x => x.CreateAdminUserAsync(It.IsAny<AdminUserOptions>(), It.IsAny<CancellationToken>()))
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
        _userManagerMock
            .Setup(x => x.CreateAdminUserAsync(adminUserOptions, It.IsAny<CancellationToken>()))
            .ThrowsAsync(innerException);
    
        // --- When ---
        Func<Task> action = () => _authUserHandler.CreateAdminUserAsync(CancellationToken.None);
    
        // --- Then ---
        await action.Should().ThrowAsync<AdminUserAlreadyExistsException>()
            .WithMessage($"The administrator user with the email '{adminUserOptions.Email}' already exists.");
    }
}