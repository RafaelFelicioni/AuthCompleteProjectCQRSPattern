using CleanArchMonolit.Application.HttpContext;
using CleanArchMonolit.Shared.Utils;
using Microsoft.AspNetCore.Http;

namespace CleanArchMonolit.Infrastructure.DataShared.HttpContextService
{
    public class HttpContextService : IHttpContextService
    {
        public HttpContextService(IHttpContextAccessor httpContext)
        {
            UserId = httpContext.HttpContext?.User?.GetUserId() ?? 0;
            ProfileId = httpContext.HttpContext?.User?.GetProfileId() ?? 0;
            ProfileName = httpContext.HttpContext?.User?.GetProfileName() ?? string.Empty;
            UserName = httpContext.HttpContext?.User?.GetUserName() ?? string.Empty;
            IsAdmin = httpContext.HttpContext?.User?.IsAdmin() ?? false;
        }

        public int UserId { get; }
        public int ProfileId { get; }
        public string ProfileName { get; }
        public string UserName { get; }
        public bool IsAdmin { get; }
    }
}
