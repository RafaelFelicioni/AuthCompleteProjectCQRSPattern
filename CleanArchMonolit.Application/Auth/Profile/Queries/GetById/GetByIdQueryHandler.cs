using CleanArchMonolit.Application.Auth.Repositories.ProfileRepositories;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Profile.Queries.GetById
{
    public sealed record GetProfileByIdQuery(int Id) : IRequest<Result<Profiles>>;
    public sealed class GetByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, Result<Profiles>>
    {
        private readonly IProfileRepository _profileRepository;

        public GetByIdQueryHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<Result<Profiles>> Handle(GetProfileByIdQuery query, CancellationToken ct)
        {
            var profile = await _profileRepository.GetById(query.Id);
            if (profile == null)
            {
                return Result<Profiles>.Fail("Ocorreu um erro ao buscar o perfil");
            }

            return Result<Profiles>.Ok(profile);
        }
    }
}
