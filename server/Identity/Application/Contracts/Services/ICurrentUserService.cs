namespace Recipes.Identity.Application.Contracts.Services
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Username { get; }
    }
}
