using CleanArchMonolit.Application.Auth.Repositories.PermissionRepositories;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Infrastructure.DataShared;
using CleanArchMonolit.Infrastruture.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchMonolit.Infrastructure.Auth.Repositories.PermissionRepositories
{
    public class PermissionRepository : BaseRepository<SystemPermission, AuthDbContext>, IPermissionRepository
    {
        private readonly AuthDbContext _context;

        public PermissionRepository(AuthDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> HasPermissionWithSameName(string permissionName)
        {
            return await GetDbSet().AnyAsync(x => x.PermissionName == permissionName);
        }

        public async Task<bool> HasPermissionWithSameCode(string code)
        {
            return await GetDbSet().AnyAsync(x => x.PermissionCode == code);
        }

        public async Task<List<SystemPermission>> GetAllPermissions(bool isAdmin)
        {
            return await GetDbSet().Where(x => isAdmin || (!isAdmin && !x.AdminOnly)).ToListAsync();
        }
    }
}
