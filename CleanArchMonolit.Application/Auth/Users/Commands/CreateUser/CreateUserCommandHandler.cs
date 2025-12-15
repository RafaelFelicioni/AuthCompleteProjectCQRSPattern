using CleanArchMonolit.Application.Auth.Repositories.ProfileRepositories;
using CleanArchMonolit.Application.Auth.Repositories.UserRepositories;
using CleanArchMonolit.Application.HttpContext;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Users.Commands.CreateUser
{
    public sealed record CreateUserCommand(int Id,
    string UserName, 
    string Email, 
    string Password,
    int ProfileId,
    string TaxId, 
    IEnumerable<int> PermissionList) : IRequest<Result<bool>>;
    
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<bool>>
    {
        private readonly IHttpContextService _httpContext;
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly PasswordHasher<User> _hasher = new();
        public CreateUserCommandHandler(IHttpContextService httpContext, IUserRepository userRepository, IProfileRepository profileRepository)
        {
            _httpContext = httpContext;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
        }

        public async Task<Result<bool>> Handle(CreateUserCommand command, CancellationToken ct)
        {
            var isAdmin = _httpContext.IsAdmin;

            var user = await _userRepository.GetByEmailAsync(command.Email);
            if (user != null)
                return Result<bool>.Fail("Usuário já existe na base de dados com este email");

            var checkTaxId = await _userRepository.CheckIfTaxIdExists(command.TaxId);
            if (checkTaxId)
                return Result<bool>.Fail("Já existe um usuário cadastrado com esse CPF, por favor corrija as informações e tente novamente.");

            var profile = await _profileRepository.GetById(command.ProfileId);
            if (profile is null)
                return Result<bool>.Fail("Perfil inválido.");

            var newUser = new User(
                id: 0, 
                active: true,
                username: command.UserName,
                mail: command.Email,
                passwordHash: string.Empty,
                profileId: command.ProfileId,
                taxId: command.TaxId,
                profile: profile
            );
            var hashedPassword = _hasher.HashPassword(user, command.Password);
            newUser.SetPasswordHash(hashedPassword);
            // Let the aggregate manage its children
            if (command.PermissionList?.Any() == true)
                newUser.AddPermissions(command.PermissionList);

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
    }
}
