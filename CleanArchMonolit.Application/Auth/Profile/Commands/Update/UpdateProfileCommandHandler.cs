using CleanArchMonolit.Application.Auth.Repositories.ProfileRepositories;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Profile.Commands.Update
{
    public sealed record UpdateProfileCommand(int Id, string ProfileName) : IRequest<Result<bool>>;
    public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<bool>>
    {
        private readonly IProfileRepository _profileRepository;

        public UpdateProfileCommandHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<Result<bool>> Handle(UpdateProfileCommand command, CancellationToken ct)
        {
            var profile = await _profileRepository.GetById(command.Id);
            if (profile == null)
            {
                return Result<bool>.Fail("Ocorreu um erro ao buscar o perfil");
            }

            var profileDup = await _profileRepository.FirstOrDefaultAsync(x => x.ProfileName == command.ProfileName && x.Id != command.Id);
            if (profileDup != null)
            {
                return Result<bool>.Fail("Já existe um perfil com este nome, por favor altere o nome e tente novamente, ou utilize o perfil: " + command.ProfileName);
            }

            profile.ChangeProfileName(command.ProfileName);
            await _profileRepository.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
    }
}
