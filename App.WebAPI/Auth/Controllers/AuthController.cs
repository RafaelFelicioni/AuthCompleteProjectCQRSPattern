using CleanArchMonolit.Application.Auth.DTO;
using CleanArchMonolit.Application.Auth.Login.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static CleanArchMonolit.Application.Auth.Login.Commands.Login.LoginCommandHandler;

namespace App.WebAPI.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
