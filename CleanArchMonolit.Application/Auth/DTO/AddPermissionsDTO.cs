namespace CleanArchMonolit.Application.Auth.DTO
{
    public class AddPermissionsDTO
    {
        public string PermissionCode { get; set; }
        public string PermissionName { get; set; }
        public bool AdminOnly { get; set; }
    }
}
