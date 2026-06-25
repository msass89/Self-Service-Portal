using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SelfServiceHub.Models.Entities;

namespace SelfServiceHub.UnitTests.TestHelpers
{
    public static class SignInManagerMockHelper
    {
        public static Mock<SignInManager<ApplicationUser>> GetMockSignInManager()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            // Mock IHttpContextAccessor
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            contextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

            // Mock IUserClaimsPrincipalFactory
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

            // Create SignInManager with mocks
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object
            );

            return signInManagerMock;
        }
    }
}
