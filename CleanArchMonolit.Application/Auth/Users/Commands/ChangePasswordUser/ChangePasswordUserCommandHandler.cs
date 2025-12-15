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

namespace CleanArchMonolit.Application.Auth.Users.Commands.ChangePasswordUser
{
    public sealed record ChangePasswordUserCommand(string OldPassword, string NewPassword) : IRequest<Result<bool>>;
    public sealed class ChangePasswordUserCommandHandler : IRequestHandler<ChangePasswordUserCommand, Result<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextService _httpContext;
        private readonly PasswordHasher<User> _hasher = new();
        public ChangePasswordUserCommandHandler(IUserRepository userRepository, IHttpContextService httpContext)
        {
            _userRepository = userRepository;
            _httpContext = httpContext;
        }

        public async Task<Result<bool>> Handle(ChangePasswordUserCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetById(_httpContext.UserId);
            if (user == null)
                return Result<bool>.Fail("Não foi possivel encontrar o usuário informado");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, command.OldPassword);
            if (result == PasswordVerificationResult.Failed)
                return Result<bool>.Fail("Senha antiga informada não corresponde a senha na base de dados, por favor tente novamente.");

            user.SetPasswordHash(_hasher.HashPassword(user, command.NewPassword));
            return Result<bool>.Ok(true);
        }
    }
}
