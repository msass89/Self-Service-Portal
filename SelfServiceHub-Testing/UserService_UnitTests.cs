using Microsoft.AspNetCore.Identity;
using Moq;
using SelfServiceHub.UnitTests.TestHelpers;
using SelfServiceHub.Services;
using SelfServiceHub.Models.Entities;

namespace SelfServiceHub.UnitTests.Services
{
    [TestClass]
    public sealed class UserService_UnitTests
    {
        [TestMethod]
        public void CreatesUserService_WhenUserManagerIsProvided()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var userService = new UserService(userManagerMock.Object);

            Assert.IsNotNull(userService);
        }

        [TestMethod]
        public void UserService_GetUserByIdAsync_ReturnsUser_WhenUserExists()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.FindByIdAsync("123"))
                .ReturnsAsync(testUser);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.GetUserByIdAsync("123").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(testUser.UserName, result.UserName);
        }

        [TestMethod]
        public void UserService_GetUserByEmailAsync_ReturnsUser_WhenUserExists()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser", Email = "testuser@example.com" };
            userManagerMock
                .Setup(x => x.FindByEmailAsync("testuser@example.com"))
                .ReturnsAsync(testUser);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.GetUserByEmailAsync("testuser@example.com").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(testUser.UserName, result.UserName);
        }

        [TestMethod]
        public void UserService_GenerateAccountConfirmationTokenAsync_ReturnsToken()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.GenerateEmailConfirmationTokenAsync(testUser))
                .ReturnsAsync("confirmation-token");

            var userService = new UserService(userManagerMock.Object);

            var result = userService.GenerateAccountConfirmationTokenAsync(testUser).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual("confirmation-token", result);
            userManagerMock.Verify(x => x.GenerateEmailConfirmationTokenAsync(testUser), Times.Once);
        }

        [TestMethod]
        public void UserService_ConfirmEmailAsync_ReturnsSuccess()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.ConfirmEmailAsync(testUser, "confirmation-token"))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.ConfirmEmailAsync(testUser, "confirmation-token").Result;

            Assert.IsTrue(result.Succeeded);
            userManagerMock.Verify(x => x.ConfirmEmailAsync(testUser, "confirmation-token"), Times.Once);
        }

        [TestMethod]
        public void UserService_IsEmailConfirmedAsync_ReturnsTrue_WhenEmailIsConfirmed()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.IsEmailConfirmedAsync(testUser))
                .ReturnsAsync(true);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.IsEmailConfirmedAsync(testUser).Result;

            Assert.IsTrue(result);
            userManagerMock.Verify(x => x.IsEmailConfirmedAsync(testUser), Times.Once);
        }

        [TestMethod]
        public void UserService_GeneratePasswordResetTokenAsync_ReturnsToken()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.GeneratePasswordResetTokenAsync(testUser))
                .ReturnsAsync("reset-token");

            var userService = new UserService(userManagerMock.Object);

            var result = userService.GeneratePasswordResetTokenAsync(testUser).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual("reset-token", result);
            userManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(testUser), Times.Once);
        }

        [TestMethod]
        public void UserService_ResetPasswordAsync_ReturnsSuccess()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.ResetPasswordAsync(testUser, "reset-token", "newpassword"))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.ResetPasswordAsync(testUser, "reset-token", "newpassword").Result;

            Assert.IsTrue(result.Succeeded);
            userManagerMock.Verify(x => x.ResetPasswordAsync(testUser, "reset-token", "newpassword"), Times.Once);
            // Note: Identity handles password hashing and security internally, so we don't need to verify that here
        }

        [TestMethod]
        public void UserService_CreateUserAsync_ReturnsSuccess()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var newUser = new ApplicationUser { UserName = "newuser", Email = "newuser@example.com" };
            userManagerMock
                .Setup(x => x.CreateAsync(newUser, "password"))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.CreateUserAsync(newUser, "password").Result;

            Assert.IsTrue(result.Succeeded);
            userManagerMock.Verify(x => x.CreateAsync(newUser, "password"), Times.Once);
        }

        [TestMethod]
        public void UserService_AddClaimAsync_ReturnsSuccess()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.AddClaimAsync(testUser, It.IsAny<System.Security.Claims.Claim>())) //It.IsAny takes a claim object consisting of claimType and claimValue
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.AddClaimAsync(testUser, "claimType", "claimValue").Result;

            Assert.IsTrue(result.Succeeded);
            userManagerMock.Verify(x => x.AddClaimAsync(testUser, It.Is<System.Security.Claims.Claim>(c => c.Type == "claimType" && c.Value == "claimValue")), Times.Once);
        }

        [TestMethod]
        public void UserService_AddUserToRoleAsync_ReturnsSuccess()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.AddToRoleAsync(testUser, "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.AddUserToRoleAsync(testUser, "Admin").Result;

            Assert.IsTrue(result.Succeeded);
            userManagerMock.Verify(x => x.AddToRoleAsync(testUser, "Admin"), Times.Once);
            // Note: Role management and security are handled by Identity, so we don't need to verify the internal workings here
        }

        [TestMethod]
        public void UserService_UpdateUserAsync_ReturnsSuccess()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.UpdateAsync(testUser))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.UpdateUserAsync(testUser).Result;

            Assert.IsTrue(result.Succeeded);
            userManagerMock.Verify(x => x.UpdateAsync(testUser), Times.Once);
        }

        [TestMethod]
        public void UserService_DeleteUserAsync_ReturnsSuccess()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();

            var testUser = new ApplicationUser { Id = "123", UserName = "testuser" };
            userManagerMock
                .Setup(x => x.DeleteAsync(testUser))
                .ReturnsAsync(IdentityResult.Success);

            var userService = new UserService(userManagerMock.Object);

            var result = userService.DeleteUserAsync(testUser).Result;

            Assert.IsTrue(result.Succeeded);
            userManagerMock.Verify(x => x.DeleteAsync(testUser), Times.Once);
        }
    }
}
