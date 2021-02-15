using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Recipes.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator Mediator;

        public ApiControllerBase(IMediator mediator) =>
            Mediator = mediator;
    }
}
