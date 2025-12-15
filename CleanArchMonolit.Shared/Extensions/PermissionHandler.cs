using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace CleanArchMonolit.Shared.Extensions
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var hasPermission = context.User.Claims.Any(c =>
                c.Type == "permissions" &&
                c.Value == requirement.Permission) || context.User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "Admin");

            if (hasPermission)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
