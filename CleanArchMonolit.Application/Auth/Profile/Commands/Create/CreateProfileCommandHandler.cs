using CleanArchMonolit.Application.Auth.Repositories.ProfileRepositories;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Profile.Commands.Create
{
    public sealed record CreateProfileCommand(string ProfileName) : IRequest<Result<bool>>;
    public sealed class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Result<bool>>
    {
        private readonly IProfileRepository _profileRepository;

        public CreateProfileCommandHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<Result<bool>> Handle(CreateProfileCommand command, CancellationToken ct)
        {
            var profile = await _profileRepository.FirstOrDefaultAsync(x => x.ProfileName == command.ProfileName);
            if (profile != null)
            {
                return Result<bool>.Fail("Perfil já existe, por favor altere o nome e tente novamente");
            }

            profile = new Profiles(0, command.ProfileName);

            await _profileRepository.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
    }
}
