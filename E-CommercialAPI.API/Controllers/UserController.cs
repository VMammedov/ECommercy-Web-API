using E_CommercialAPI.Application.Features.Commands.User.CreateUser;
using E_CommercialAPI.Application.Features.Commands.User.GoogleLogin;
using E_CommercialAPI.Application.Features.Commands.User.LoginUser;
using E_CommercialAPI.Application.Features.Commands.User.RefreshTokenLogin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_CommercialAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest request)
        {
            LoginUserCommandResponse? response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody]RefreshTokenLoginCommandRequest request)
        {
            RefreshTokenLoginCommandResponse? response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse response = await _mediator.Send(googleLoginCommandRequest);
            return Ok(response);
        }
    }
}
