using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Dtos.Identity.Commands;
using Recipes.Application.Dtos.Identity.Queries;

namespace Recipes.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiControllerBase
    {
        public AdminController(IMediator mediator)
            : base(mediator) { } 

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await Mediator.Send(new AdminGetUsersQuery());
            return Ok(users);
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await Mediator.Send(new AdminGetRolesQuery());
            return Ok(roles);
        }

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
