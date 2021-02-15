using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Dtos.Identity.Commands;
using Recipes.Application.Dtos.Identity.Queries;
using System.Threading.Tasks;

namespace Recipes.Identity.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsController : ApiControllerBase
    {
        public AccountsController(IMediator mediator) 
            : base(mediator) { } 

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var token = await Mediator.Send(command);
            return Ok(token);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var token = await Mediator.Send(command);
            return Ok(token);
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await Mediator.Send(new GetCurrentUserRolesQuery());
            return Ok(roles);
        }

        [HttpPost("Refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshJwtTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("UpdateCurrentUser")]
        public async Task<IActionResult> UpdateCurrentUser(UpdateCurrentUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("ConfirmResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmResetPassword(ConfirmResetPasswordCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
