using CleanArchMonolit.Application.Auth.DTO;
using CleanArchMonolit.Application.Auth.Users.Commands.ChangePasswordUser;
using CleanArchMonolit.Application.Auth.Users.Commands.CreateUser;
using CleanArchMonolit.Application.Auth.Users.Commands.UpdateUser;
using CleanArchMonolit.Application.Auth.Users.Queries.GetUser;
using CleanArchMonolit.Application.Auth.Users.Queries.GetUsersGrid;
using CleanArchMonolit.Application.Auth.Users.Queries.SelectUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WebAPI.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Update")]
        //[Authorize(Policy = "test")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("ChangePassword")]
        [Authorize(Policy = "")]
        public async Task<IActionResult> ChangePasswordUser(string oldPassword, string newPassword)
        {
            var command = new ChangePasswordUserCommand(oldPassword, newPassword);
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("GetUserInfo")]
        [Authorize(Policy = "")]
        public async Task<IActionResult> GetUserInfo([FromQuery] int id)
        {
            var query = new GetUserQuery(id);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("GetUsersGrid")]
        [Authorize(Policy = "")]
        public async Task<IActionResult> GetUsersGrid(GetUsersGridDTO dto)
        {
            var query = new GetUsersGridQuery(dto);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("SelectUsers")]
        [Authorize(Policy = "")]
        public async Task<IActionResult> SelectUsers(string term)
        {
            var query = new SelectUsersQuery(term);
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
