using System.ComponentModel.DataAnnotations;

namespace CleanArchMonolit.Domain.Auth.Entities
{
    public class SystemPermission
    {
        private readonly List<UserSystemPermissions> _permissions = new();
        private SystemPermission() { }
        public SystemPermission(int id, bool adminOnly, string permissionCode, string permissionName, string permissionDefinition, IEnumerable<UserSystemPermissions>? permissions = null)
        {
            Id = id;
            PermissionCode = permissionCode;
            PermissionName = permissionName;
            PermissionDefinition = permissionDefinition;
            if (permissions != null) _permissions.AddRange(permissions);
        }
        public int Id { get; set; }
        [MaxLength(4)]
        public string PermissionCode { get; private set; } = null!;
        [MaxLength(100)]
        public string PermissionName { get; private set; } = null!;
        [MaxLength(250)]
        public string PermissionDefinition { get; private set; } = null!;
        public bool AdminOnly { get; private set; }
        public IReadOnlyCollection<UserSystemPermissions> UserPermissions => _permissions;

        public void AddPermission(int systemPermissionId)
        {
            if (_permissions.Any(p => p.SystemPermissionId == systemPermissionId)) return;
            _permissions.Add(new UserSystemPermissions(systemPermissionId));
        }

        public void AddPermissions(IEnumerable<int> systemPermissionIds)
        {
            foreach (var id in systemPermissionIds.Distinct())
                AddPermission(id);
        }

        public void ChangePermissionCode(string permissionCode) => PermissionCode = permissionCode;
        public void ChangePermissionName(string permissionName) => PermissionName = permissionName;
        public void ChangeAdminOnly() => AdminOnly = !AdminOnly;
    }
}
