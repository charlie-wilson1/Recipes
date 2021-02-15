using System.Threading.Tasks;
using Recipes.Application.Dtos.Notifications.SendGrid;

namespace Recipes.Application.Contracts.Notifications.SendGrid
{
    public interface IEmailService
    {
        Task SendConfirmResetPasswordTemplatedEmail(SendGridTemplatedEmailDto dto);
        Task SendResetPasswordTemplatedEmail(SendGridTemplatedEmailDto dto);
        Task SendTemplatedEmail(SendGridTemplatedEmailDto dto);
    }
}
