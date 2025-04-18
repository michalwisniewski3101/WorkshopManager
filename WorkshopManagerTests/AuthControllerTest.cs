using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using WorkshopManager.api.Model; 
using WorkshopManager.api.Controllers;

namespace WorkshopManager.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private Mock<IConfiguration> _configurationMock;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _userManagerMock = GetMockUserManager<IdentityUser>();
            _signInManagerMock = GetMockSignInManager<IdentityUser>(_userManagerMock.Object);
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("TestKeyForJwtToken1234567890123456");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
            var durationSectionMock = new Mock<IConfigurationSection>();
            durationSectionMock.Setup(x => x.Value).Returns("60");
            _configurationMock.Setup(x => x.GetSection("Jwt:DurationInMinutes")).Returns(durationSectionMock.Object);

            _controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object);
        }


        private static Mock<UserManager<TUser>> GetMockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

  
        private static Mock<SignInManager<TUser>> GetMockSignInManager<TUser>(UserManager<TUser> userManager) where TUser : class
        {
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var userClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();
            return new Mock<SignInManager<TUser>>(userManager, contextAccessor.Object, userClaimsPrincipalFactory.Object, null, null, null, null);
        }

        #region Testy rejestracji

        [Test]
        public async Task Register_ValidModel_ReturnsOk()
        {
             
            var model = new RegisterModel
            {
                Username = "testuser",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Password = "Test@1234"
            };

            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Success);

             
            var result = await _controller.Register(model);

             
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Rejestracja powiod�a si�", okResult.Value);
        }

        [Test]
        public async Task Register_InvalidModelState_ReturnsBadRequest()
        {
             
            _controller.ModelState.AddModelError("Error", "B��d walidacji");
            var model = new RegisterModel
            {
                Username = "testuser",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Password = "Test@1234"
            };

             
            var result = await _controller.Register(model);

             
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Register_FailedCreation_ReturnsBadRequestWithErrors()
        {
             
            var model = new RegisterModel
            {
                Username = "testuser",
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Password = "Test@1234"
            };

            var identityError = new IdentityError { Description = "B��d przy tworzeniu u�ytkownika" };
            var failedResult = IdentityResult.Failed(identityError);
            _userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), model.Password))
                .ReturnsAsync(failedResult);

             
            var result = await _controller.Register(model);

             
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult.Value);

        }

        #endregion

        #region Testy logowania

        [Test]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
             
            var model = new LoginModel
            {
                Username = "testuser",
                Password = "Test@1234",
                RememberMe = false
            };

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var testUser = new IdentityUser { UserName = model.Username };
            _userManagerMock
                .Setup(x => x.FindByNameAsync(model.Username))
                .ReturnsAsync(testUser);
            _userManagerMock
                .Setup(x => x.GetRolesAsync(testUser))
                .ReturnsAsync(new List<string> { "User" });

             
            var result = await _controller.Login(model);

             
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<TokenModel>(okResult.Value);
            var tokenModel = okResult.Value as TokenModel;
            Assert.IsFalse(string.IsNullOrEmpty(tokenModel.Token));
            Assert.IsFalse(string.IsNullOrEmpty(tokenModel.RefreshToken));
        }

        [Test]
        public async Task Login_InvalidModelState_ReturnsBadRequest()
        {
             
            _controller.ModelState.AddModelError("Error", "B��d walidacji");
            var model = new LoginModel
            {
                Username = "testuser",
                Password = "Test@1234",
                RememberMe = false
            };

             
            var result = await _controller.Login(model);

             
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
             
            var model = new LoginModel
            {
                Username = "testuser",
                Password = "WrongPassword",
                RememberMe = false
            };

            _signInManagerMock
                .Setup(x => x.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

             
            var result = await _controller.Login(model);

             
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }

        #endregion

        #region Test wylogowania

        [Test]
        public async Task Logout_ReturnsOk()
        {
             
            _signInManagerMock
                .Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

             
            var result = await _controller.Logout();

             
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Wylogowanie powiod�o si�", okResult.Value);
        }

        #endregion
    }
}
