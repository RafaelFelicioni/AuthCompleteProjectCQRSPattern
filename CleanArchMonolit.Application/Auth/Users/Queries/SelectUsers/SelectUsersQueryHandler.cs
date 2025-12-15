using CleanArchMonolit.Application.Auth.Repositories.UserRepositories;
using CleanArchMonolit.Application.HttpContext;
using CleanArchMonolit.Shared.DTO;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Users.Queries.SelectUsers
{
    public sealed record SelectUsersQuery(string searchTerm) : IRequest<Result<List<SelectPatternDTO>>>;
    public sealed class SelectUsersQueryHandler : IRequestHandler<SelectUsersQuery, Result<List<SelectPatternDTO>>>
    {
        private readonly IHttpContextService _httpContext;
        private readonly IUserRepository _userRepository;

        public SelectUsersQueryHandler(IHttpContextService httpContext, IUserRepository userRepository)
        {
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        public async Task<Result<List<SelectPatternDTO>>> Handle(SelectUsersQuery query, CancellationToken ct)
        {
            var isAdmin = _httpContext.IsAdmin;

            var usersSelect = await _userRepository.GetSelectUser(query.searchTerm, isAdmin);
            if (usersSelect != null)
            {
                return Result<List<SelectPatternDTO>>.Ok(usersSelect);
            }

            return Result<List<SelectPatternDTO>>.Fail("Ocorreu um erro ao buscar os usuários");
        }
    }
}
