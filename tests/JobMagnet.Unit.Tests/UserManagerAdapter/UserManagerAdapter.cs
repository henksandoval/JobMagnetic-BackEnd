// using System.Security.Claims;
// using AwesomeAssertions;
// using JobMagnet.Application.UseCases.Auth.DTO;
// using JobMagnet.Application.UseCases.Auth.Ports.EmailDTO;
// using JobMagnet.Infrastructure.ExternalServices.Identity.Entities;
// using JobMagnet.Infrastructure.Services.EmailService.Interfaces;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Configuration;
// using Moq;
//
// namespace JobMagnet.Unit.Tests.UserManagerAdapter;
//
// public class UserManagerAdapterTests
// {
//     private readonly Mock<UserManager<ApplicationIdentityUser>> _mockUserManager;
//     private readonly Mock<IConfiguration> _mockConfiguration;
//     private readonly Mock<IEmailService> _mockEmailService;
//     private readonly Infrastructure.Services.Auth.UserManagerAdapter _userManagerAdapter;
//
//     public UserManagerAdapterTests()
//     {
//         var userStoreMock = new Mock<IUserStore<ApplicationIdentityUser>>();
//         _mockUserManager = new Mock<UserManager<ApplicationIdentityUser>>(   userStoreMock.Object,
//             null,
//             null,
//             null,
//             null, 
//             null,                
//             null,                   
//             null,                   
//             null);
//         _mockConfiguration = new Mock<IConfiguration>();
//         _mockEmailService = new Mock<IEmailService>();
//         _userManagerAdapter = new Infrastructure.Services.Auth.UserManagerAdapter( _mockUserManager.Object, _mockConfiguration.Object, _mockEmailService.Object);
//     }
//     
//     [Fact]
//     public async Task RegisterAsync_ShouldSendConfirmationEmail_WhenRegistrationIsSuccessful()
//     {
//         // --- Given ---
//         var userModel = new UserModelCredentials 
//         { 
//             Email = "test@exitoso.com", 
//             Password = "PasswordValido123!" 
//         };
//         
//         MailCommand capturedMailRequest = null;
//         
//         _mockUserManager.Setup(um => um.FindByEmailAsync(userModel.Email))
//             .ReturnsAsync((ApplicationIdentityUser)null);
//         
//         _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationIdentityUser>(), userModel.Password))
//             .ReturnsAsync(IdentityResult.Success);
//         
//         _mockUserManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationIdentityUser>()))
//             .ReturnsAsync("fake-confirmation-token");
//         
//         _mockConfiguration.Setup(c => c["ClientApp:ConfirmationUrl"])
//             .Returns("https://mi-frontend.com/confirmar");
//         
//         _mockConfiguration.Setup(c => c["JWT:Key"])
//             .Returns("una-clave-secreta-muy-larga-para-pruebas-de-al-menos-256-bits");
//         
//         _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationIdentityUser>()))
//             .ReturnsAsync(new List<string>()); 
//
//         _mockUserManager.Setup(um => um.GetClaimsAsync(It.IsAny<ApplicationIdentityUser>()))
//             .ReturnsAsync(new List<Claim>());
//         
//         _mockEmailService
//             .Setup(es => es.SendEmailAsync(It.IsAny<MailCommand>()))
//             .Callback<MailCommand>(mail => capturedMailRequest = mail) 
//             .Returns(Task.CompletedTask);
//
//         // --- When ---
//         var userTokenResult = await _userManagerAdapter.RegisterAsync(userModel);
//
//         // --- Then ---
//         
//         _mockEmailService.Verify(
//             es => es.SendEmailAsync(It.IsAny<MailCommand>()), 
//             Times.Once);
//         
//         userTokenResult.Should().NotBeNull();
//         userTokenResult.Token.Should().NotBeNullOrEmpty();
//     
//         capturedMailRequest.Should().NotBeNull();
//         capturedMailRequest.ToEmail.Should().Be(userModel.Email);
//         capturedMailRequest.Subject.Should().Be("Confirma tu cuenta en JobMagnet");
//         capturedMailRequest.Body.Should().Contain("¡Bienvenido a JobMagnet!");
//         capturedMailRequest.Body.Should().Contain("href=\"https://mi-frontend.com/confirmar?email=test@exitoso.com&token=");
//     }
// }