using CleanArchMonolit.Application.Auth.DTO;
using CleanArchMonolit.Application.Auth.Permission.Commands.CreatePermission;
using CleanArchMonolit.Application.Auth.Permission.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static CleanArchMonolit.Application.Auth.Permission.Commands.CreatePermission.CreatePermissionCommandHandler;

namespace App.WebAPI.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> CreatePermission(CreatePermissionCommand command)
        {
            var resp = await _mediator.Send(command);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPermissions(GetAllPermissionsQuery query)
        {
            var resp = await _mediator.Send(query);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }
    }
}
