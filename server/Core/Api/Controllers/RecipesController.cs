using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipes.Core.Api.Services;
using Recipes.Core.Application.Recipes.Commands;
using Recipes.Core.Application.Recipes.Queries;

namespace Recipes.Core.Api.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    [Authorize]
    public class RecipesController : BaseApiController
    {
        public RecipesController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string searchQuery, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] int skippedResults)
        {
            var query = new GetRecipesQuery(searchQuery, pageSize, pageNumber, skippedResults);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]string id)
        {
            var query = new GetRecipeQuery(id);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateRecipeCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRecipeCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            var command = new DeleteRecipeCommand(id);
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("Image")]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile formFile)
        {
            var file = await formFile.ToUploadedFileDto();
            var command = new UploadImageCommand(file);
            var url = await Mediator.Send(command);
            return Ok(url);
        }
    }
}
