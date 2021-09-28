using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Core.Application.ShoppingCarts.Commands;
using Recipes.Core.Application.ShoppingCarts.Queries;
using System.Threading.Tasks;

namespace Recipes.Core.Api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : BaseApiController
    {
        public ShoppingCartController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetShoppingCartQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateShoppingCartCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("recipe")]
        public async Task<IActionResult> AddRecipe(AddRecipeToShoppingCartCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("items/add")]
        public async Task<IActionResult> AddItem(AddItemToShoppingCartCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("items/update")]
        public async Task<IActionResult> UpdateItem(UpdateShoppingCartItemQuantityCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("items/remove")]
        public async Task<IActionResult> RemoveItem(RemoveShoppingCartItemCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("clear")]
        public async Task<IActionResult> ClearCart(ClearShoppingCartCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
