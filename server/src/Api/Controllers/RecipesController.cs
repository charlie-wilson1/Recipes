using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.Dtos;
using Recipes.Application.Dtos.Recipes.Commands;
using Recipes.Application.Dtos.Recipes.Queries;

namespace Recipes.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RecipesController : ApiControllerBase
    {
        public RecipesController(IMediator mediator)
            : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string searchQuery, [FromQuery] int resultsPerPage, [FromQuery] int? startNumber)
        {
            var paginatedRequest = new PaginatedRequest
            {
                SearchQuery = searchQuery,
                ResultsPerPage = resultsPerPage,
                StartNumber = startNumber
            };

            var query = new GetAllRecipesQuery { Request = paginatedRequest };
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var query = new GetRecipeByIdQuery(id);
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateRecipeCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateRecipeCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteRecipeCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("Ingredients")]
        public async Task<IActionResult> GetIngredients([FromQuery] string searchQuery)
        {
            var query = new GetIngredientNamesQuery(searchQuery);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("Image")]
        public async Task<IActionResult> UploadImage(UploadImageCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
