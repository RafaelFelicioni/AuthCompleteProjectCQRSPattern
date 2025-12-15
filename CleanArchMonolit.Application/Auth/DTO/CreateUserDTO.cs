namespace CleanArchMonolit.Application.Auth.DTO
{
    public class CreateUserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int ProfileId { get; set; }
        public string TaxId { get; set; }
        public List<int> PermissionList { get; set; } = new List<int>();
    }
}
