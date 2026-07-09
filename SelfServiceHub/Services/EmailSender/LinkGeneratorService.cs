
namespace SelfServiceHub.Services.EmailSender
{
    public class LinkGeneratorService : ILinkGenerator
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LinkGeneratorService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateLink(string page, string userId, string token)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            return _linkGenerator.GetUriByPage(
                httpContext,
                page,
                values: new
                {
                    area = "Identity",
                    userId,
                    token
                },
                scheme: httpContext?.Request?.Scheme ?? "https"
            );
        }
    }
}