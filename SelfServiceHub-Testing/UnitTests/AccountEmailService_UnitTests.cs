
using SelfServiceHub.UnitTests.TestHelpers;
using SelfServiceHub.Models.Entities;

namespace SelfServiceHub.UnitTests.Services
{
    [TestClass]
    public sealed class AccountEmailService_UnitTests
    {        
        [TestMethod]
        public void CreatesAccountEmailService_WhenDependenciesAreProvided()
        {
            var helper = AccountEmailServiceMockHelper.Create();
            var accountEmailService = helper.AccountEmailService;

            Assert.IsNotNull(accountEmailService);
        }

        [TestMethod]
        public async Task UnitTest_SendConfirmationEmailAsync_EnqueueEmail()
        {
            var helper = AccountEmailServiceMockHelper.Create();
            var accountEmailService = helper.AccountEmailService;

            var user = new ApplicationUser
            {
                Id = "17",
                Email = "test@example.com"
            };

            await accountEmailService.SendConfirmationEmailAsync(user);

            Assert.AreEqual(1, helper.FakeEmailQueue.Messages.Count);

            var msg = helper.FakeEmailQueue.Messages[0];

            Assert.AreEqual("test@example.com", msg.To);
            Assert.IsNotNull(msg.Link);
            Assert.AreEqual("https://example.com/confirm", msg.Link);
        }

        [TestMethod]
        public async Task UnitTest_SendPasswordResetEmailAsync_EnqueueEmail()
        {
            var helper = AccountEmailServiceMockHelper.Create();
            var accountEmailService = helper.AccountEmailService;

            var user = new ApplicationUser
            {
                Id = "15",
                Email = "test2@example.com"
            };

            await accountEmailService.SendPasswordResetEmailAsync(user);

            Assert.AreEqual(1, helper.FakeEmailQueue.Messages.Count);

            var msg = helper.FakeEmailQueue.Messages[0];

            Assert.AreEqual("test2@example.com", msg.To);
            Assert.IsNotNull(msg.Link);
            Assert.AreEqual("https://example.com/confirm", msg.Link);
        }
    }
}