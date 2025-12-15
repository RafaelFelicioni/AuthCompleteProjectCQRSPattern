using CleanArchMonolit.Application.Auth.Interfaces.Token;
using CleanArchMonolit.Application.Auth.Repositories.UserRepositories;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Shared.Responses;
using CleanArchMonolit.Shared.Settings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static CleanArchMonolit.Application.Auth.Login.Commands.Login.LoginCommandHandler;

namespace CleanArchMonolit.Application.Auth.Login.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<string>>;
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _hasher = new();
        private readonly IGenerateTokenService _generateTokenService;

        public LoginCommandHandler(IUserRepository userRepository, IGenerateTokenService generateTokenService)
        {
            _userRepository = userRepository;
            _generateTokenService = generateTokenService;
        }

        public async Task<Result<string>> Handle(LoginCommand command, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(command.Email) || string.IsNullOrEmpty(command.Password))
            {
                return Result<string>.Fail("Email e senha são obrigatórios.");
            }

            var user = await _userRepository.GetByEmailAsync(command.Email);
            if (user == null)
                return Result<string>.Fail("Usuário ou senha inválidos.");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, command.Password);
            if (result == PasswordVerificationResult.Failed)
                return Result<string>.Fail("Usuário ou senha inválidos.");

            var token = _generateTokenService.GenerateToken(user);
            return Result<string>.Ok(token);
        }
    }
}
