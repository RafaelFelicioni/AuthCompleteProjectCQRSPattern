using CleanArchMonolit.Application.Auth.Repositories.ProfileRepositories;
using CleanArchMonolit.Domain.Auth.Entities;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Profile.Queries.GetAll
{
    public sealed record GetAllQuery() : IRequest<Result<List<Profiles>>>;
    public sealed class GetAllQueryHandler : IRequestHandler<GetAllQuery, Result<List<Profiles>>>
    {
        private readonly IProfileRepository _profileRepository;

        public GetAllQueryHandler(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<Result<List<Profiles>>> Handle(GetAllQuery query, CancellationToken ct)
        {
            var profiles = await _profileRepository.GetAll();
            if (!profiles.Any())
            {
                return Result<List<Profiles>>.Fail("Ocorreu um erro ao buscar os perfis");
            }

            return Result<List<Profiles>>.Ok(profiles);
        }
    }
}
