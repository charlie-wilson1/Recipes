namespace Recipes.Application.Contracts.Identity
{
    public interface ICurrentUserService
    {
        string UserId { get; set; }
        string Username { get; set; }
    }
}
