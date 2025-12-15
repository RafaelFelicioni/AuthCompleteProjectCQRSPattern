using CleanArchMonolit.Application.Auth.DTO;
using CleanArchMonolit.Application.Auth.Repositories.UserRepositories;
using CleanArchMonolit.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMonolit.Application.Auth.Users.Queries.GetUser
{
    public sealed record GetUserQuery(int Id) : IRequest<Result<UserDTO>>;
    public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserDTO>> Handle(GetUserQuery query, CancellationToken ct)
        {
            var user = await _userRepository.GetById(query.Id);
            if (user == null)
                return Result<UserDTO>.Fail("Não foi possivel encontrar o usuário informado");

            UserDTO userDTO = user;
            return Result<UserDTO>.Ok(user);
        }
    }
}
