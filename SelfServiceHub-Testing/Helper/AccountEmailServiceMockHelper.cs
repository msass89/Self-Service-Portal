using Microsoft.AspNetCore.Http;
using Moq;
using SelfServiceHub.Services.EmailSender;
using SelfServiceHub.Services;
using SelfServiceHub.Models.DTO;

namespace SelfServiceHub.UnitTests.TestHelpers
{
    public static class AccountEmailServiceMockHelper
    {
        public class Result
        {
            public AccountEmailService AccountEmailService { get; set; }
            public FakeEmailQueue FakeEmailQueue { get; set; }
        }

        public class FakeEmailQueue : IEmailQueue
        {
            public List<EmailQueueMessage> Messages { get; } = new();

            public async Task EnqueueAsync(EmailQueueMessage message)
            {
                Messages.Add(message);
            }

            public async Task<EmailQueueMessage> DequeueAsync(CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }


        public static Result Create()
        {
            var userManagerMock = UserManagerMockHelper.GetMockUserManager();
            var userService = new UserService(userManagerMock.Object);

            var fakeEmailQueue = new FakeEmailQueue();

            var linkGenerator = new Mock<ILinkGenerator>();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            httpContextAccessor
                .Setup(a => a.HttpContext)
                .Returns(new DefaultHttpContext());

            linkGenerator
                .Setup(x => x.GenerateLink(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("https://example.com/confirm");


            var accountEmailService = new AccountEmailService(
                userService,
                fakeEmailQueue,
                linkGenerator.Object,
                httpContextAccessor.Object
            );

            return new Result
            {
                AccountEmailService = accountEmailService,
                FakeEmailQueue = fakeEmailQueue
            };
        }
    }
}