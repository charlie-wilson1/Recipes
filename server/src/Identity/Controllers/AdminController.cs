using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Dtos.Identity.Commands;

namespace Recipes.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiControllerBase
    {
        public AdminController(IMediator mediator)
            : base(mediator) { } 

        [HttpPost("Register")]
        public async Task<IActionResult> Register(AdminRegisterUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("UpdateRoles")]
        public async Task<IActionResult> UpdateRoles(UpdateRolesCommand command)
        {
            var roles = await Mediator.Send(command);
            return Ok(roles);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetUserPassword(AdminResetUserPasswordCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
