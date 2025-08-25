using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.UseCases.Auth;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Ports;
using JobMagnet.Domain.Aggregates;
using Moq;

namespace JobMagnet.Unit.Tests.UseCases;

public class AuthUserHandlerShould
{
    private readonly IFixture _fixture;
    private readonly Mock<IUserManagerAdapter> _userManagerAdapterMock;
    private readonly AuthUserHandler _authUserHandler;

    public AuthUserHandlerShould()
    {
        _fixture = new Fixture();
        _userManagerAdapterMock = new Mock<IUserManagerAdapter>();
        _authUserHandler =  new AuthUserHandler(_userManagerAdapterMock.Object);
    }
    
    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsUserToken()
    {
        // --- Given ---
        var loginDto = _fixture.Create<LoginDto>();
        var expectedToken = new UserToken
        {
            Token = "un_jwt_token_valido",
            Expiration = DateTime.UtcNow.AddHours(1)
        };

        _userManagerAdapterMock.Setup(x => x.LoginAsync(loginDto))
            .ReturnsAsync(expectedToken);
    
        var handler = new AuthUserHandler(_userManagerAdapterMock.Object);

        // --- When  ---
        var result = await handler.LoginAsync(loginDto);

        // --- Then  ---
        result.Should().NotBeNull();
        result.Token.Should().BeEquivalentTo(expectedToken.Token);
    }
    
    [Fact]
    public async Task LoginAsync_WhenAdapterReturnsNull_ReturnsNull()
    {
        // --- Given ---
        var loginDto = _fixture.Create<LoginDto>();
        var handler = new AuthUserHandler(_userManagerAdapterMock.Object);
        _userManagerAdapterMock.Setup(x => x.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync((UserToken)null);

        // --- When ---
        var result = await handler.LoginAsync(loginDto);

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
        var handler = new AuthUserHandler(_userManagerAdapterMock.Object);
        var loginDto = new LoginDto { Email = email, Password = password };
        
        // --- When ---
        Func<Task> action = () => handler.LoginAsync(loginDto);
        
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
        _userManagerAdapterMock.Setup(x => x.CreateAdminUserAsync(It.IsAny<AdminUser>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedToken);
        
        // --- When ---
        var result = await _authUserHandler.CreateAdminUserAsync(CancellationToken.None);

        // --- Then ---
        result.Should().NotBeNull();
        result.Token.Should().BeEquivalentTo(expectedToken.Token);
        result.Expiration.Should().BeCloseTo(expectedToken.Expiration, TimeSpan.FromSeconds(1));
    }
}