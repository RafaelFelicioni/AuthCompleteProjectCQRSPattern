using CleanArchMonolit.Application.Auth.Repositories.UserRepositories;
using CleanArchMonolit.Application.Auth.Validators;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Users.Commands.UpdateUser
{
    public sealed record UpdateUserCommand(int Id, string OldPassword, string NewPassword, string UserName, string Email, int ProfileId, List<int> PermissionList) : IRequest<Result<bool>>;
    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<bool>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> Handle(UpdateUserCommand command, CancellationToken ct)
        {
            var emailTaken = await _userRepository
                .FirstOrDefaultAsync(x => x.Id != command.Id && x.Mail == command.Email);
            if (emailTaken != null)
                return Result<bool>.Fail("Este email já está cadastrado para outro usuário, por favor altere o email e tente novamente");

            var user = await _userRepository
                .FindAsync(command.Id);

            if (user == null)
                return Result<bool>.Fail("Não foi possivel encontrar o usuário informado, por favor entre em contato com o suporte.");

            user.ChangeEmail(command.Email);
            user.Rename(command.UserName);
            user.ChangeProfile(command.ProfileId);

            user.ReplacePermissions(command.PermissionList ?? Enumerable.Empty<int>());

            await _userRepository.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
    }
}
