using CleanArchMonolit.Application.Auth.Repositories.PermissionRepositories;
using CleanArchMonolit.Application.HttpContext;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Permission.Queries.GetAll
{
    public sealed record GetAllPermissionsQuery() : IRequest<Result<List<SystemPermission>>>;
    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, Result<List<SystemPermission>>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IHttpContextService _httpContextService;

        public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository, IHttpContextService httpContextService) 
        {
            _permissionRepository = permissionRepository;
            _httpContextService = httpContextService;
        }

        public async Task<Result<List<SystemPermission>>> Handle(GetAllPermissionsQuery query, CancellationToken ct)
        {
            var isAdmin = _httpContextService.IsAdmin;
            var allPermissions = await _permissionRepository.GetAllPermissions(isAdmin);
            return Result<List<SystemPermission>>.Ok(allPermissions);
        }
    }
}
