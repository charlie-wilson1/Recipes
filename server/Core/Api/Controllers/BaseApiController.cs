using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Recipes.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly IMediator Mediator;

        public BaseApiController(IMediator mediator) =>
            Mediator = mediator;
    }
}
