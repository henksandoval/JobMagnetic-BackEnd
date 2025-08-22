using AwesomeAssertions;
using JobMagnet.Application.UseCases.Auth;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using Moq;

namespace JobMagnet.Unit.Tests.UseCases;

public class AuthUserHandlerShould
{
    private readonly Mock<IUserManagerAdapter> _userAdapter = new();
    
    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsUserToken()
    {
        // --- Given ---
        var loginDto = new LoginDto { Email = "test@email.com", Password = "password" };
        var expectedToken = new UserToken
        {
            Token = "un_jwt_token_valido",
            Expiration = DateTime.UtcNow.AddHours(1)
        };

        _userAdapter.Setup(x => x.LoginAsync(loginDto))
            .ReturnsAsync(expectedToken);
    
        var handler = new AuthUserHandler(_userAdapter.Object);

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
        var loginDto = new LoginDto { Email = "test@email.com", Password = "password" };
        var handler = new AuthUserHandler(_userAdapter.Object);
        _userAdapter.Setup(x => x.LoginAsync(It.IsAny<LoginDto>())).ReturnsAsync((UserToken)null);

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
        var handler = new AuthUserHandler(_userAdapter.Object);
        var loginDto = new LoginDto { Email = email, Password = password };
        
        // --- When ---
        Func<Task> action = () => handler.LoginAsync(loginDto);
        
        // --- Then ---
        await action.Should().ThrowAsync<ArgumentException>()
            .WithMessage("The email and password cannot be null, empty, or contain only spaces.");
      
    }
}