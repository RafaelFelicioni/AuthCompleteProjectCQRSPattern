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
using static CleanArchMonolit.Application.Auth.Permission.Commands.CreatePermission.CreatePermissionCommandHandler;

namespace CleanArchMonolit.Application.Auth.Permission.Commands.CreatePermission
{
    public record CreatePermissionCommand(string PermissionCode, string PermissionName, bool AdminOnly, string PermissionDefinition) : IRequest<Result<bool>>;
    public sealed class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Result<bool>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IHttpContextService _httpContextService;

        public CreatePermissionCommandHandler(IPermissionRepository permissionRepository, IHttpContextService httpContextService)
        {
            _permissionRepository = permissionRepository;
            _httpContextService = httpContextService;
        }

        public async Task<Result<bool>> Handle(CreatePermissionCommand command, CancellationToken ct)
        {
            var isAdmin = _httpContextService.IsAdmin;
            if (!isAdmin)
                return Result<bool>.Fail("Ocorreu um erro ao criar a pemissão, apenas usuários administradores podem criar permissões");

            var hasPermissionWithSameName = await _permissionRepository.HasPermissionWithSameName(command.PermissionName);
            if (hasPermissionWithSameName)
                return Result<bool>.Fail($"Já existe uma permissão com o nome {command.PermissionName}");

            var hasPermissionWIthSameCode = await _permissionRepository.HasPermissionWithSameCode(command.PermissionCode);
            if (hasPermissionWithSameName)
                return Result<bool>.Fail($"Já existe uma permissão com o codigo {command.PermissionCode}");
            var newPermission = new SystemPermission(0, command.AdminOnly,command.PermissionCode, command.PermissionName, command.PermissionDefinition);

            await _permissionRepository.AddAsync(newPermission);
            await _permissionRepository.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
    }
}
