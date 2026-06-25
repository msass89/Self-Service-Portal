using Microsoft.AspNetCore.Identity;
using Moq;
using SelfServiceHub.Services.Auth;
using SelfServiceHub.UnitTests.TestHelpers;

namespace SelfServiceHub.UnitTests.Services
{
    [TestClass]
    public sealed class AuthService_UnitTests
    {
        [TestMethod]
        public void CreatesAuthService_WhenSignInManagerIsProvided()
        {
            var signInManagerMock = SignInManagerMockHelper.GetMockSignInManager();

            var authService = new AuthService(signInManagerMock.Object);

            Assert.IsNotNull(authService);
        }

        [TestMethod]
        public void AuthService_LoginAsync_ReturnsSuccess_WhenCredentialsAreValid()
        {
            var signInManagerMock = SignInManagerMockHelper.GetMockSignInManager();

            signInManagerMock
                .Setup(x => x.PasswordSignInAsync("testuser", "password", false, true))
                .ReturnsAsync(SignInResult.Success);

            var authService = new AuthService(signInManagerMock.Object);

            var result = authService.LoginAsync("testuser", "password", false).Result;

            Assert.AreEqual(LoginResult.Success, result);
        }

        [TestMethod]
        public void AuthService_LogoutAsync_CallsSignOutAsyncOnce()
        {
            var signInManagerMock = SignInManagerMockHelper.GetMockSignInManager();

            var authService = new AuthService(signInManagerMock.Object);

            // Call LogoutAsync
            authService.LogoutAsync().Wait();

            // Verify that SignOutAsync was called once on the SignInManager
            signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);
        }
    }
}
