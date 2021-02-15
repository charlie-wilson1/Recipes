namespace Recipes.Application.Dtos.Identity.Responses
{
    public class JwtAuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
