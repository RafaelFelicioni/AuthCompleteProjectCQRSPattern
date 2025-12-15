using CleanArchMonolit.Application.Auth.DTO;
using CleanArchMonolit.Application.Auth.Repositories.UserRepositories;
using CleanArchMonolit.Application.HttpContext;
using CleanArchMonolit.Shared.DTO.Grid;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Users.Queries.GetUsersGrid
{
    public sealed record GetUsersGridQuery(GetUsersGridDTO filter) : IRequest<Result<GridResponseDTO<ReturnUsersGridDTO>>>;
    public sealed class GetUsersGridQueryHandler : IRequestHandler<GetUsersGridQuery, Result<GridResponseDTO<ReturnUsersGridDTO>>>
    {
        private readonly IHttpContextService _httpContext;
        private readonly IUserRepository _userRepository;

        public GetUsersGridQueryHandler(IHttpContextService httpContext, IUserRepository userRepository)
        {
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        public async Task<Result<GridResponseDTO<ReturnUsersGridDTO>>> Handle(GetUsersGridQuery query, CancellationToken ct)
        {
            var userIsAdmin = _httpContext.IsAdmin;

            var usersGrid = await _userRepository.GetUserGrid(query.filter);
            return Result<GridResponseDTO<ReturnUsersGridDTO>>.Ok(usersGrid);
        }
    }
}
