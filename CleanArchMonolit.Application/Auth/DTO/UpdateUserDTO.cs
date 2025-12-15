namespace CleanArchMonolit.Application.Auth.DTO
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int ProfileId { get; set; }

        public List<int> PermissionList { get; set; } = new List<int>();
    }
}
