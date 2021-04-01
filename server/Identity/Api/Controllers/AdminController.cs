using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Identity.Application.Identity.Commands;
using Recipes.Identity.Application.Identity.Queries;

namespace Recipes.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseApiController
    {
        public AdminController(IMediator mediator) : base(mediator) { }


        [HttpPost("Invitation")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Invitation(CreateInvitationCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers([FromQuery]UsersQuery command)
        {
            var users = await Mediator.Send(command);
            return Ok(users);
        }

        [HttpPatch("Roles")]
        public async Task<IActionResult> UpdateRoles(AdminUpdateUserRolesCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
