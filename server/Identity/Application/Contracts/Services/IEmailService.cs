using System.Threading;
using System.Threading.Tasks;
using Recipes.Identity.Application.Common.Models;

namespace Recipes.Identity.Application.Contracts.Services
{
    public interface IEmailService
    {
        Task SendConfirmResetPasswordEmail(SendGridTemplatedEmailDto dto, CancellationToken cancellationToken);
        Task SendResetPasswordEmail(SendGridTemplatedEmailDto dto, CancellationToken cancellationToken);
        Task SendInvitationEmail(SendGridTemplatedEmailDto dto, CancellationToken cancellationToken);
        SendGridTemplatedEmailDto CreateTokenizedEmailDto(string email, string token, string endpoint);
    }
}
