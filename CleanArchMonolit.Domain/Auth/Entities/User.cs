using System.ComponentModel.DataAnnotations;

namespace CleanArchMonolit.Domain.Auth.Entities
{
    public class User
    {
        private readonly List<UserSystemPermissions> _permissions = new();

        private User() { } 

        public User(
            int id,
            bool active,
            string username,
            string mail,
            string passwordHash,
            int profileId,
            string taxId,
            Profiles profile,
            IEnumerable<UserSystemPermissions>? permissions = null)
        {
            Id = id;
            Active = active;
            Username = username;
            Mail = mail;
            PasswordHash = passwordHash;
            ProfileId = profileId;
            TaxId = taxId;
            Profile = profile;

            if (permissions != null) _permissions.AddRange(permissions);
        }

        public int Id { get; private set; }
        public bool Active { get; private set; }
        [MaxLength(500)]
        public string Username { get; private set; } = null!;
        [MaxLength(500)]
        public string Mail { get; private set; } = null!;
        [MaxLength(800)]
        public string PasswordHash { get; private set; } = null!;
        public int ProfileId { get; private set; }
        [MaxLength(20)]
        public string TaxId { get; private set; } = null!;
        public Profiles Profile { get; private set; } = null!;

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

        public void SetPasswordHash(string hash) => PasswordHash = hash;
        public void ChangeEmail(string email) => Mail = email;
        public void Rename(string username) => Username = username;
        public void ChangeProfile(int profileId) => ProfileId = profileId;
        public void Activate() => Active = true;
        public void Deactivate() => Active = false;
        public void GrantPermission(int systemPermissionId)
        {
            if (_permissions.Any(p => p.SystemPermissionId == systemPermissionId)) return;
            _permissions.Add(new UserSystemPermissions(systemPermissionId));
        }

        public void RevokePermission(int systemPermissionId)
        {
            var p = _permissions.FirstOrDefault(x => x.SystemPermissionId == systemPermissionId);
            if (p != null) _permissions.Remove(p);
        }

        public void ReplacePermissions(IEnumerable<int> desiredIds)
        {
            if (!desiredIds.Any()) 
            {
                return;
            }

            var desired = desiredIds.Distinct().ToHashSet();
            var current = _permissions.Select(p => p.SystemPermissionId).ToHashSet();

            // remove extras
            foreach (var toRemove in current.Except(desired).ToList())
                RevokePermission(toRemove);

            // add missing
            foreach (var toAdd in desired.Except(current))
                GrantPermission(toAdd);
        }
    }
}
