using System.Security.Claims;

namespace CleanArchMonolit.Shared.Utils
{
    public static class ContextHelper
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "";
        }

        public static string GetProfileName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? "";
        }

        public static int GetProfileId(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.Claims.FirstOrDefault(x => x.Type == "ProfileId")?.Value ?? "0");
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            var role = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (role == "Admin")
            {
                return true;
            }
            return false;
        }
    }
}
