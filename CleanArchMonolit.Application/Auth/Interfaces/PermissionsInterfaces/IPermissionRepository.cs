using CleanArchMonolit.Application.Auth.Interfaces.Interface;
using CleanArchMonolit.Domain.Auth.Entities;

namespace CleanArchMonolit.Application.Auth.Repositories.PermissionRepositories
{
    public interface IPermissionRepository : IRepository<SystemPermission>
    {
        Task<bool> HasPermissionWithSameCode(string code);
        Task<bool> HasPermissionWithSameName(string permissionName);
        Task<List<SystemPermission>> GetAllPermissions(bool isAdmin);
    }
}
