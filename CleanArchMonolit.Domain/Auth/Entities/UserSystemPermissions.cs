namespace CleanArchMonolit.Domain.Auth.Entities
{
    public class UserSystemPermissions
    {
        private UserSystemPermissions() { }

        public UserSystemPermissions(int systemPermissionId)
        {
            SystemPermissionId = systemPermissionId;
        }

        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int SystemPermissionId { get; private set; }

        public virtual User User { get; private set; } = null!;
        public virtual SystemPermission SystemPermission { get; private set; } = null!;
    }
}
