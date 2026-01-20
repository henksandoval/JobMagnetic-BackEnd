using System.Security.Claims;
using AwesomeAssertions;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Ports.EmailDTO;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Infrastructure.ExternalServices.Identity.Entities;
using JobMagnet.Infrastructure.Services.EmailService.Interfaces;
using JobMagnet.Shared.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace JobMagnet.Unit.Tests.UserManagerAdapter;

public class UserManagerAdapterTests
{
    private readonly Mock<UserManager<ApplicationIdentityUser>> _mockUserManager;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Mock<IUnitOfWork>  _mockUnitOfWork;
    private readonly Mock<IGuidGenerator> _mockGuidGenerator;
    private readonly Infrastructure.Services.Auth.UserManagerAdapter _userManagerAdapter;

    public UserManagerAdapterTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationIdentityUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationIdentityUser>>(userStoreMock.Object,
            null,
            null,
            null,
            null, 
            null,                
            null,                   
            null,                   
            null);
        _mockConfiguration = new Mock<IConfiguration>();
        _mockEmailService = new Mock<IEmailService>();
        _mockUnitOfWork  = new Mock<IUnitOfWork>();
        _mockGuidGenerator = new Mock<IGuidGenerator>();
        _userManagerAdapter = new Infrastructure.Services.Auth.UserManagerAdapter(
            _mockUserManager.Object,
            _mockConfiguration.Object,
            _mockEmailService.Object,
            _mockUnitOfWork.Object,
            _mockGuidGenerator.Object);
    }
    
     [Fact]
     public async Task RegisterAsync_ShouldSendConfirmationEmail_WhenRegistrationIsSuccessful()
     {
         // --- Given ---
         var userModel = new UserModelCredentialsDto 
         { 
             Email = "test@exitoso.com", 
             Password = "PasswordValido123!" 
         };
         
         MailCommand capturedMailRequest = null;
         var expectedDomainId = Guid.NewGuid();
         
         _mockUserManager.Setup(um => um.FindByEmailAsync(userModel.Email))
             .ReturnsAsync((ApplicationIdentityUser)null);
         
         _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationIdentityUser>(), userModel.Password))
             .Callback<ApplicationIdentityUser, string>((user, pass) => 
             {
                 user.Id = Guid.NewGuid();
             })
             .ReturnsAsync(IdentityResult.Success);
         
         _mockGuidGenerator.Setup(g => g.NewGuid()).Returns(expectedDomainId);
         
         _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
             .ReturnsAsync(1);
         
         _mockUserManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationIdentityUser>()))
             .ReturnsAsync("fake-confirmation-token");
         
         _mockConfiguration.Setup(c => c["ClientApp:ConfirmationUrl"])
             .Returns("https://mi-frontend.com/confirmar");
         
         _mockConfiguration.Setup(c => c["JWT:Key"])
             .Returns("una-clave-secreta-muy-larga-para-pruebas-de-al-menos-256-bits");
         
         _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationIdentityUser>()))
             .ReturnsAsync(new List<string>()); 

         _mockUserManager.Setup(um => um.GetClaimsAsync(It.IsAny<ApplicationIdentityUser>()))
             .ReturnsAsync(new List<Claim>());
         
         _mockEmailService
             .Setup(es => es.SendEmailAsync(It.IsAny<MailCommand>()))
             .Callback<MailCommand>(mail => capturedMailRequest = mail) 
             .Returns(Task.CompletedTask);

         // --- When ---
         var userTokenResult = await _userManagerAdapter.RegisterAsync(userModel, CancellationToken.None);

         // --- Then ---
         
         _mockEmailService.Verify(
             es => es.SendEmailAsync(It.IsAny<MailCommand>()), 
             Times.Once);
         
         userTokenResult.Should().NotBeNull();
         userTokenResult.Token.Should().NotBeNullOrEmpty();
     
         capturedMailRequest.Should().NotBeNull();
         capturedMailRequest.ToEmail.Should().Be(userModel.Email);
         capturedMailRequest.Subject.Should().Be("Confirma tu cuenta en JobMagnet");
         capturedMailRequest.Body.Should().Contain("¡Bienvenido a JobMagnet!");
         capturedMailRequest.Body.Should().Contain("href=\"https://mi-frontend.com/confirmar?email=test@exitoso.com&token=");
     }
}