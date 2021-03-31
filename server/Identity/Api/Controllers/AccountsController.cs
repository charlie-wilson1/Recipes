using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Identity.Application.Identity.Commands;

namespace Recipes.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : BaseApiController
    {
        public AccountsController(IMediator mediator) 
            : base(mediator) { }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(AuthenticateCommand command)
        {
            var token = await Mediator.Send(command);
            return Ok(token);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var token = await Mediator.Send(command);
            return Ok(token);
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var token = await Mediator.Send(command);
            return Ok(token);
        }

        [HttpPatch("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("SendResetPasswordEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> SendResetPasswordEmail(SendResetPasswordEmailCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("TokenResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> TokenResetPassword(ResetPasswordWithTokenCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("User")]
        public async Task<IActionResult> UpdateUser(UpdateCurrentUserCommand command)
        {
            var user = await Mediator.Send(command);
            return Ok(user);
        }
    }
}
