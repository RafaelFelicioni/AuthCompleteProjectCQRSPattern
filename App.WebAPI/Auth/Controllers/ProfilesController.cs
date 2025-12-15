using CleanArchMonolit.Application.Auth.DTO;
using CleanArchMonolit.Application.Auth.Profile.Commands.Create;
using CleanArchMonolit.Application.Auth.Profile.Commands.Update;
using CleanArchMonolit.Application.Auth.Profile.Queries.GetAll;
using CleanArchMonolit.Application.Auth.Profile.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.WebAPI.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        [Authorize(Policy = "")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllQuery());
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetById")]
        //[Authorize(Policy = "")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            
            var result = await _mediator.Send(new GetProfileByIdQuery(id));
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("AddProfile")]
        //[Authorize(Policy = "")]
        public async Task<IActionResult> AddProfile([FromBody] CreateProfileCommand dto)
        {
            var result = await _mediator.Send(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("UpdateProfile")]
        //[Authorize(Policy = "")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand dto)
        {
            var result = await _mediator.Send(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
